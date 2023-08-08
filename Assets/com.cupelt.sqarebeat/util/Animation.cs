using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace com.cupelt.sqarebeat.util
{
    public class Animation : MonoBehaviour
    {
        [HideInInspector] public bool readyAnimation = true;
        [HideInInspector] public bool active = false;
    
        public Ease animaton = Tweening.Linear;
    
        public float offset;
        public float delay = 0.5f;
    
        public Vector3 movePos;

        //public bool idleOriginpos = true;

        private Vector3 _originPos;
        private bool _returnAni = false;

        private void Awake()
        {
            _originPos = GetComponent<RectTransform>().anchoredPosition3D;
            //GetComponent<RectTransform>().anchoredPosition3D = _originPos;
        }

        public void setActive(bool _active)
        {
            if (active == _active || !readyAnimation)
            {
                if (!readyAnimation)
                {
                    _returnAni = !_returnAni;
                    active = _active;
                }
                return;
            }

            active = _active;
            StartCoroutine(startAnimation(active));
        }

        protected virtual void setAlpha(float alpha) { }

        IEnumerator startAnimation(bool _active)
        {
            readyAnimation = false;
            RectTransform trans = gameObject.GetComponent<RectTransform>();

            float time = -offset;
        
            float reverse = _active ? 1f : -1f;
            float min = _active ? 0f : 1f;

            while (time / delay < 1 || (_returnAni && time / delay > 0))
            {
                if (_returnAni) time -= Time.deltaTime;
                else time += Time.deltaTime;
                
                float fixedTime = time / delay;
                fixedTime = Tweening.FixedTime(fixedTime);
                trans.anchoredPosition3D = _originPos + ((min + animaton(fixedTime) * reverse) * movePos) - (movePos * (reverse + min * 2));
                setAlpha(min + animaton(fixedTime) * reverse);

                yield return null;
            }

            readyAnimation = true;
            _returnAni = false;
        }
    }
    
#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Animation), true)]
    public class LocalizationGUI : Editor
    {
        private Animation _target;
        
        private readonly Dictionary<Ease, string> _easeList = new Dictionary<Ease, string>
        {
            {Tweening.Linear, "Linear"},
            {Tweening.InQuad, "InQuad"},
            {Tweening.InCubic, "InCubic"},
            {Tweening.InQuart, "InQuart"},
            {Tweening.InQuint, "InQuint"},
            {Tweening.InSine, "InSine"},
            {Tweening.InExpo, "InExpo"},
            {Tweening.InCirc, "InCirc"},
            {Tweening.InElastic, "InElastic"},
            {Tweening.InBack, "InBack"},
            {Tweening.OutQuad, "OutQuad"},
            {Tweening.OutCubic, "OutCubic"},
            {Tweening.OutQuart, "OutQuart"},
            {Tweening.OutQuint, "OutQuint"},
            {Tweening.OutSine, "OutSine"},
            {Tweening.OutExpo, "OutExpo"},
            {Tweening.OutCirc, "OutCirc"},
            {Tweening.OutElastic, "OutElastic"},
            {Tweening.OutBack, "OutBack"},
            {Tweening.InOutQuad, "InOutQuad"},
            {Tweening.InOutCubic, "InOutCubic"},
            {Tweening.InOutQuart, "InOutQuart"},
            {Tweening.InOutQuint, "InOutQuint"},
            {Tweening.InOutSine, "InOutSine"},
            {Tweening.InOutExpo, "InOutExpo"},
            {Tweening.InOutCirc, "InOutCirc"},
            {Tweening.InOutElastic, "InOutElastic"},
            {Tweening.InOutBack, "InOutBack"}
        };

        private void OnEnable()
        {
            _target = target as Animation;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            List<Ease> keys = new List<Ease>(_easeList.Keys);

            _target.animaton = keys[EditorGUILayout.Popup("Animation Ease", keys.IndexOf(_target.animaton), _easeList.Values.ToArray())];
        }
    }
#endif
}
