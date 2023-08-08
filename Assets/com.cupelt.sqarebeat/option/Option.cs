using System.IO;
using com.cupelt.sqarebeat.localization;
using com.cupelt.sqarebeat.util;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace com.cupelt.sqarebeat.option
{
    //Language Option -
    public enum LanguageEnum        { en_us }

    //Graphic Option -
    //Diaply Option
    public enum DisplayModeEnum     { FullScreen, Windowd, BorderlessWindowed }
    public enum DisplayFrameEnum    { Synchronization, Unlimited, Custom }

    //Graphic Option
    public enum PresetQualityEnum   { Low, Middle, High, Custom } // ?

    public enum TextureQualityEnum  { Low, Middle, High }
    public enum AntiAliasingEnum    { Disable, MSAA, FXAA, SMAA }
    public enum BloomEnum           { Disable, Low, Middle, High }
    //Other Setting
    
    [System.Serializable]
    public class Option
    {
        public LanguageEnum lang = LanguageEnum.en_us;

        public DisplayModeEnum displayMode = DisplayModeEnum.FullScreen;
        public DisplayFrameEnum displayFrame = DisplayFrameEnum.Synchronization;
        public int resolution = 0;
        private int _width;
        private int _height;
        public int maxFrameRate = -1;
        public bool vsync = false;

        public TextureQualityEnum textureQuality = TextureQualityEnum.High;
        public AntiAliasingEnum antiAliasing = AntiAliasingEnum.FXAA;
        public BloomEnum bloom = BloomEnum.Middle;

        public bool isShowFrameRate = false;
        public bool isShowTime = false;

        public void save()
        {
            string path = Application.persistentDataPath + "/Option.json";
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(path, json);
        }

        public Resolution getResolution()
        {
            if (_width.Equals(0) || _height.Equals(0))
                return Screen.currentResolution;
            else
                return new Resolution
                {
                    width = _width,
                    height = _height,
                    refreshRate = Screen.currentResolution.refreshRate
                };
        }

        public static Option load()
        {
            string path = Application.persistentDataPath + "/Option.json";
            
            if (!File.Exists(path))
            {
                new Option().save();
            }

            string json = File.ReadAllText(path);

            Option o = JsonUtility.FromJson<Option>(json);
            o.resolution = 0;

            Resolution[] resolutions = Util.getFixedResolutions();

            for (int res = 0; res < resolutions.Length; res++)
                if (o._width.Equals(resolutions[res].width) && o._height.Equals(resolutions[res].height))
                {
                    o.resolution = res;
                    break;
                }

            return o;
        }
        
        public void applyResolution()
        {
            Resolution[] resolutions = Util.getFixedResolutions();
            switch (displayMode)
            {
                case DisplayModeEnum.FullScreen: Screen.SetResolution(resolutions[resolution].width, resolutions[resolution].height, FullScreenMode.ExclusiveFullScreen); break;
                case DisplayModeEnum.Windowd: Screen.SetResolution(resolutions[resolution].width, resolutions[resolution].height, FullScreenMode.Windowed); break;
                case DisplayModeEnum.BorderlessWindowed: Screen.SetResolution(resolutions[resolution].width, resolutions[resolution].height, FullScreenMode.FullScreenWindow); break;
            }
            _width = resolutions[resolution].width;
            _height = resolutions[resolution].height;
            save();
        }

        public void applyAudio()
        {
            
        }
        
        public void applyOption()
        {
            //Language Option -
            Localization.locale = lang.ToString();
            Localization.loadLocale();
            Localization.applyLocale();

            //Graphic Option -
            //Diaply Option

            switch (displayFrame)
            {
                case DisplayFrameEnum.Synchronization: Application.targetFrameRate = Screen.currentResolution.refreshRate; break;
                case DisplayFrameEnum.Unlimited: Application.targetFrameRate = 3000; break;
                case DisplayFrameEnum.Custom: Application.targetFrameRate = maxFrameRate; break;
            }

            //Graphic
            switch (textureQuality)
            {
                case TextureQualityEnum.High: QualitySettings.globalTextureMipmapLimit = 0; break;
                case TextureQualityEnum.Middle: QualitySettings.globalTextureMipmapLimit = 1; break;
                case TextureQualityEnum.Low: QualitySettings.globalTextureMipmapLimit = 2; break;
            }

            PostProcessLayer.Antialiasing antiMode = PostProcessLayer.Antialiasing.None;
            switch (antiAliasing)
            {
                case AntiAliasingEnum.FXAA: antiMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing; break;
                case AntiAliasingEnum.SMAA: antiMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing; break;
            }
            if (antiAliasing.Equals(AntiAliasingEnum.MSAA))
            {
                QualitySettings.antiAliasing = 4;
            }
            else
            {
                QualitySettings.antiAliasing = 0;
                foreach (Camera cam in Camera.allCameras)
                {
                    PostProcessLayer postProcess = cam.gameObject.GetComponent<PostProcessLayer>();
                    if (postProcess)
                    {
                        postProcess.antialiasingMode = antiMode;
                    }
                }
            }

            if (vsync) QualitySettings.vSyncCount = 1;
            else QualitySettings.vSyncCount = 0;


            foreach (Camera cam in Camera.allCameras)
            {
                Bloom postBloom;
                cam.gameObject.GetComponent<PostProcessVolume>().profile.TryGetSettings(out postBloom);
                if (postBloom)
                {
                    float intensity = 0f;

                    switch (bloom)
                    {
                        case BloomEnum.Low: intensity = 1f; break;
                        case BloomEnum.Middle: intensity = 2.5f; break;
                        case BloomEnum.High: intensity = 5f; break;
                    }
                    postBloom.intensity.value = intensity;
                }
            }

            //Other Option

            save();
        }
    }
}