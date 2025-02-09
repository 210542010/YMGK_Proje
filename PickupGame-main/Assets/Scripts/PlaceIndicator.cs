using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceIndicator : MonoBehaviour
{
    GameObject indicatorObject;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits=new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = FindAnyObjectByType<ARRaycastManager>();
        indicatorObject = transform.GetChild(0).gameObject;
        indicatorObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var ray= new Vector2 (Screen.width/2, Screen.height/2);
        if (raycastManager.Raycast(ray,hits,TrackableType.Planes))
        {
            Pose hitPose = hits[0].pose;
            transform.position= hitPose.position; 
            transform.rotation= hitPose.rotation;

            if (!indicatorObject.activeInHierarchy)
            {
                indicatorObject.SetActive(true);
            }
        }
    }
}
