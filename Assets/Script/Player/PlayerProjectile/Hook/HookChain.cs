using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class HookChain : MonoBehaviour
    {
        [Header("Chains"), SerializeField] private Chain[] _chains;
        public Chain[] Chains => _chains;
        public float GetChainSize()
        {
            float totalSize = 0f;
            foreach(Chain chain in _chains)
            {
                totalSize += chain.Scale;
            }
            
            return totalSize;
        }
    }
}

