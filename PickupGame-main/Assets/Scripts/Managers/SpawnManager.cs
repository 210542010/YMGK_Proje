using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public GameObject cubePrefab;
    [SerializeField] private GameObject treePrefab;
    private Tree spawnedTree;
    public TextMeshProUGUI cubeNameText;

    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    public Camera ARCam;
    [SerializeField] private Transform indicator;
    //private List<ARRaycastHit> hits = new List<ARRaycastHit>();


    public void SpawnItems()
    {
        if (spawnedTree == null)
        {
            return;
        }
        foreach (var spawnLocation in spawnedTree.spawnLocations)
        {
            Instantiate(cubePrefab, spawnLocation.position, Quaternion.identity);
        }
    }

    public void ShowTreeModel()
    {
        Instantiate(treePrefab, indicator.position, Quaternion.identity);
    }

    void CollectItems()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Fruit"))
                    {
                        FruitControlManager.instance.SetFruitAmount(hit.collider.gameObject.GetComponent<Fruit>().fruit);
                    }
                }
            }
        }
    }
}
