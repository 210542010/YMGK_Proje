using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Fruit")]
public class FruitSO : ScriptableObject
{
    public string FruitName;
    public Transform prefab;
    public AudioClip clip;
}
