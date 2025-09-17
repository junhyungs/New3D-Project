using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIK : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(0f, 120f * Time.deltaTime, 0f);
        transform.localRotation = transform.localRotation * rotation;
    }
}
