using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FruitList")]
public class FruitTypeSO : ScriptableObject
{
    public List<FruitSO> FruitList;
}
