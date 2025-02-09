using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ItemOperator : MonoBehaviour
{
    
    [SerializeField] Transform objectToFollow;
    public void MoveThroughCamera()
    {
        transform.position = Vector3.Lerp(transform.position, objectToFollow.transform.position, Time.deltaTime);
    }
}

