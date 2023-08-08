using System;
using UnityEngine;

namespace com.cupelt.sqarebeat.scenes
{
    /// <summary>
    /// This is "Global Game Manager"
    /// Manage framerate, game start time, user data, DontDestroyObject etc. here.
    /// </summary>
    public class GlobalManager : MonoBehaviour
    {
        public static Int64 startTime;
        public static bool isAfk = false;

        public static int FrameRate
        {
            get { return (int)(1.0f / ((GlobalManager)FindObjectOfType(typeof(GlobalManager))).GetDeltaTime()); }
        }

        private Int64 _lastIdleTime = 0;
        private float _deltaTime = 0f;

        private bool _instanceExists;
        
        private void Awake()
        {
            // Translation the GameObject between scenes. destroy and duplicates
            if (!_instanceExists)
            {
                _instanceExists = true;
                DontDestroyOnLoad(gameObject);
            }
            else if (FindObjectsOfType(GetType()).Length > 1)
            {
                Destroy(gameObject);
            }
        }
        void Start()
        {
            startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        private void LateUpdate()
        {
            if(Input.anyKey){
                _lastIdleTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + 300000;
            }
            
            isAfk = _lastIdleTime < DateTimeOffset.Now.ToUnixTimeMilliseconds();
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        }

        public float GetDeltaTime() => _deltaTime;
    }
}