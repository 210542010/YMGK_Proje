using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private GameObject currentItem;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            //Trigger MoveThroughCamera func from ItemOperator
            currentItem = other.gameObject;
            currentItem.GetComponent<ItemOperator>().MoveThroughCamera();
        }
    }
}
