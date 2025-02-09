using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public Transform[] spawnLocations;
    void Start()
    {
        //Projenin ilerleyiþine göre SpawnItems fonksiyonuna direkt olarak Aðacýn kendisini parametre olarak gönderebiliriz.
        SpawnManager.Instance.SpawnItems();
    }

}
