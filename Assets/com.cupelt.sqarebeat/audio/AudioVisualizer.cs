using UnityEngine;

namespace com.cupelt.sqarebeat.audio
{
    public class AudioVisualizer : MonoBehaviour
    {
        public AudioSource Audio;
        public GameObject bar;

        public float smoothSpeed = 8f;

        private void Awake()
        {
            for (int i = 0; i < 128; i++)
            {
                GameObject clone = Instantiate(bar);
                clone.transform.parent = gameObject.transform;
                clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(1920.0f / 128 * i, 0);
                clone.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }

        void Update()
        {
            float[] spectrumData = new float[128];

            Audio.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);

            for (int i = 1; i < spectrumData.Length; i++)
            {
                if (!transform.childCount.Equals(spectrumData.Length))
                    return;

                Vector3 smoothedPosition = Vector3.Lerp(transform.GetChild(i - 1).localScale, new Vector3(1, spectrumData[i] * 250 + 1, 1), smoothSpeed * Time.deltaTime);
                transform.GetChild(i - 1).localScale = smoothedPosition;
            }
        }
    }
}
