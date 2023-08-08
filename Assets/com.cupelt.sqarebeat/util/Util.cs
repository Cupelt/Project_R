using System;
using System.Collections;
using System.Collections.Generic;
using NLayer;
using UnityEngine;

namespace com.cupelt.sqarebeat.util
{
    public static class Util
    {
        public static Component copyComponent(Component original, GameObject destination)
        {
            System.Type type = original.GetType();
            bool active = destination.active;
            destination.SetActive(false);
            Component copy = destination.AddComponent(type);
            // Copied fields can be restricted with BindingFlags
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            destination.SetActive(active);
            return copy;
        }
        public static Vector3 getWorldPoint(RectTransform trans)
        {
            Transform parent = trans.parent;
            Vector3 fixedPos = trans.anchoredPosition3D;
            while (parent)
            {
                fixedPos += parent.GetComponent<RectTransform>().anchoredPosition3D;
                parent = parent.parent;
            }

            return fixedPos;
        }

        public static int changeEnum(Type t, int value, int increase)
        {
            int length = Enum.GetValues(t).Length;
            return changeCycleValue(length, value, increase);
        }
        public static int changeCycleValue(int length, int value, int increase)
        {
            value += increase;
            if (length < 1) return 0;
            if (value < 0)
                return  (value + 1) % length + (length - 1);
            return value % length;
        }

        public static int[] getResolutionRatio(int width, int height)
        {
            int max, min, gcd = 0;
            if (width > height)         { max = width; min = height; }
            else if (width > height)    { max = height; min = width; }
            else return new int[]       { 1, 1 };

            while (max % min != 0)
            {
                int temp = max % min;
                max = min;
                min = temp;
            }

            gcd = min;

            int[] ratio = new int[] { width / gcd, height / gcd };
            if (ratio[0].Equals(8) && ratio[1].Equals(5)) return new int[] { width / gcd * 2, height / gcd * 2 };
            else return ratio;
        }

        public static Resolution[] getFixedResolutions()
        {
            Resolution[] resolutions = Screen.resolutions;
            Array.Reverse(resolutions);
            List<Resolution> fixedResolution = new List<Resolution>();
            for (int i = 0; i < resolutions.Length; i++)
            {
                bool isContinue = false;
                foreach (Resolution res in fixedResolution)
                {
                    if (res.width.Equals(resolutions[i].width) && res.height.Equals(resolutions[i].height))
                    {
                        isContinue = true;
                        break;
                    }
                }
                if (isContinue) continue;
                else fixedResolution.Add(resolutions[i]);
            }

            return fixedResolution.ToArray();
        }

        public static Vector3 lerpVector(Vector3 from, Vector3 to, float time)
        {
	        return new Vector3(Mathf.Lerp(from.x, to.x, time), Mathf.Lerp(from.y, to.y, time), Mathf.Lerp(from.z, to.z, time));
        }
        
        public static AudioClip LoadMp3(string filePath)
        {
	        string filename = System.IO.Path.GetFileNameWithoutExtension(filePath);

	        MpegFile mpegFile = new MpegFile(filePath);

	        // assign samples into AudioClip
	        AudioClip ac = AudioClip.Create(filename,
		        (int)(mpegFile.Length / sizeof(float) / mpegFile.Channels),
		        mpegFile.Channels,
		        mpegFile.SampleRate,
		        false,
		        data => { int actualReadCount = mpegFile.ReadSamples(data, 0, data.Length); },
		        position => { mpegFile = new MpegFile(filePath); });

	        return ac;
        }

    }

    public delegate float Ease(float t);

    public static class Tweening
    {
	    public static float FixedTime(float t) => Mathf.Max(Mathf.Min(t, 1f), 0f);

	    public static float Linear(float t) => t;

		public static float InQuad(float t) => t * t;
		public static float OutQuad(float t) => 1 - InQuad(1 - t);
		public static float InOutQuad(float t)
		{
			if (t < 0.5) return InQuad(t * 2) / 2;
			return 1 - InQuad((1 - t) * 2) / 2;
		}

		public static float InCubic(float t) => t * t * t;
		public static float OutCubic(float t) => 1 - InCubic(1 - t);
		public static float InOutCubic(float t)
		{
			if (t < 0.5) return InCubic(t * 2) / 2;
			return 1 - InCubic((1 - t) * 2) / 2;
		}

		public static float InQuart(float t) => t * t * t * t;
		public static float OutQuart(float t) => 1 - InQuart(1 - t);
		public static float InOutQuart(float t)
		{
			if (t < 0.5) return InQuart(t * 2) / 2;
			return 1 - InQuart((1 - t) * 2) / 2;
		}

		public static float InQuint(float t) => t * t * t * t * t;
		public static float OutQuint(float t) => 1 - InQuint(1 - t);
		public static float InOutQuint(float t)
		{
			if (t < 0.5) return InQuint(t * 2) / 2;
			return 1 - InQuint((1 - t) * 2) / 2;
		}

		public static float InSine(float t) => (float)-Math.Cos(t * Math.PI / 2);
		public static float OutSine(float t) => (float)Math.Sin(t * Math.PI / 2);
		public static float InOutSine(float t) => (float)(Math.Cos(t * Math.PI) - 1) / -2;

		public static float InExpo(float t) => (float)Math.Pow(2, 10 * (t - 1));
		public static float OutExpo(float t) => 1 - InExpo(1 - t);
		public static float InOutExpo(float t)
		{
			if (t < 0.5) return InExpo(t * 2) / 2;
			return 1 - InExpo((1 - t) * 2) / 2;
		}

		public static float InCirc(float t) => -((float)Math.Sqrt(1 - t * t) - 1);
		public static float OutCirc(float t) => 1 - InCirc(1 - t);
		public static float InOutCirc(float t)
		{
			if (t < 0.5) return InCirc(t * 2) / 2;
			return 1 - InCirc((1 - t) * 2) / 2;
		}

		public static float InElastic(float t) => 1 - OutElastic(1 - t);
		public static float OutElastic(float t)
		{
			float p = 0.3f;
			return (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t - p / 4) * (2 * Math.PI) / p) + 1;
		}
		public static float InOutElastic(float t)
		{
			if (t < 0.5) return InElastic(t * 2) / 2;
			return 1 - InElastic((1 - t) * 2) / 2;
		}

		public static float InBack(float t)
		{
			float s = 1.70158f;
			return t * t * ((s + 1) * t - s);
		}
		public static float OutBack(float t) => 1 - InBack(1 - t);
		public static float InOutBack(float t)
		{
			if (t < 0.5) return InBack(t * 2) / 2;
			return 1 - InBack((1 - t) * 2) / 2;
		}
    }
}
