using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class Chain : MonoBehaviour
    {
        public string Test;

        private void OnEnable()
        {
            Test = "XX";
        }

        public float Scale
        {
            get
            {
                return transform.localScale.x;
            }
        }
    }
}

