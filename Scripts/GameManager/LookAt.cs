using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private bool targetIsPlayer = true;
    [SerializeField] private bool invert;
    void Start()
    {
        if (targetIsPlayer) {
            target = GameObject.FindGameObjectWithTag("CameraPlayer").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (invert) {
            transform.LookAt(target);
        } else {
            transform.LookAt(target);
            transform.Rotate(0, 180, 0);
        }        
    }
}
