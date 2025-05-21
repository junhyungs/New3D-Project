using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_BoredPlayer : MonoBehaviour
{
    private Animator _animator;
    private WaitForSeconds _triggerTimer = new WaitForSeconds(50f);
    private int _stretch = Animator.StringToHash("Stretch");

    void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(StretchAnimation()); 
    }

    private IEnumerator StretchAnimation()
    {
        while (true)
        {
            yield return _triggerTimer;
            _animator.SetTrigger(_stretch);
        }
    }
}
