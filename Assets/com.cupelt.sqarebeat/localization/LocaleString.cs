using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// 

namespace com.cupelt.sqarebeat.localization
{
    public class LocaleString : Localization
    {
        public Text text;
        public string[] value;

        public string localeText = "Locale is not Found X(";

        public bool isUpdateText = false;
        public float updateFrequency = 1f;

        void OnEnable()
        {
            changeLocale();
            if (isUpdateText) StartCoroutine(update());
        }
    
        private IEnumerator update()
        {
            while (gameObject.activeSelf)
            {
                if (value.Length > 0)
                    text.text = fixedText(localeText, value);

                yield return new WaitForSeconds(updateFrequency);
            }
        }

        public override void changeLocale()
        {
            localeText = getTranslation();
            text.text = localeText;
        }
    }
}
