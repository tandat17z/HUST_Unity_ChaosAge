using System;
using DatSystem.utils;
using UnityEngine;

namespace ChaosAge.manager
{
    public class TimeManager : Singleton<TimeManager>
    {
        private float _startTime;

        protected override void OnAwake()
        {
            _startTime = PlayerPrefs.GetFloat("START_TIME", 0);
        }

        public TimeSpan GetTime()
        {
            return DateTime.Now.TimeOfDay;
        }

        private void Update() {
            
        }
    }
}
