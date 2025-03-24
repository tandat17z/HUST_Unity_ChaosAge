using System.Collections;
using System.Collections.Generic;
using ChaosAge.Data;
using UnityEngine;

namespace ChaosAge.Battle
{
    public class BattleUnit : MonoBehaviour
    {
        public EUnitType Type = EUnitType.barbarian;

        private Vector3 lastPosition = Vector3.zero;
        private int _i = -1;
        private int _id = 0;

        public void Initialize(int index, int id)
        {
            _i = index;
            _id = id;
            lastPosition = transform.position;
        }

        private void Update()
        {
            if (transform.position != lastPosition)
            {
                Vector3 direction = transform.position - lastPosition;
                lastPosition = transform.position;
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}

