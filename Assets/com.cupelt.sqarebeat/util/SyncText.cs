using UnityEngine;
using UnityEngine.UI;

namespace com.cupelt.sqarebeat.util
{
    public class SyncText : MonoBehaviour
    {
        public Text text;
        void Update()
        {
            if (!GetComponent<Text>().text.Equals(text.text)) GetComponent<Text>().text = text.text;
        }
    }
}
