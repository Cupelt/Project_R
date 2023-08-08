using UnityEngine;
using UnityEngine.UI;

namespace com.cupelt.sqarebeat.util
{
    public class AnimationImage : Animation
    {
        public bool fadeInOut = true;
        
        protected override void setAlpha(float alpha)
        {
            if (!fadeInOut) return;

            Color color = GetComponent<Image>().color;
            color.a = alpha;
            GetComponent<Text>().color = color;
        }
    }
}
