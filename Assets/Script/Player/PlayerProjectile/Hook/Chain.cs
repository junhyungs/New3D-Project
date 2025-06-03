using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class Chain : MonoBehaviour
    {
        public float Scale
        {
            get
            {
                return transform.localScale.x;
            }
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    bool disable = other.gameObject.layer == LayerMask.NameToLayer("Hookshot_Fly");
        //    if (disable)
        //        gameObject.SetActive(false);
        //}
    }
}

