using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FruitControlManager : MonoBehaviour
{
    public static FruitControlManager instance { get; private set; }

    [SerializeField]
    private Dictionary<FruitSO, int> _fruitPieceAmount;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        _fruitPieceAmount = new Dictionary<FruitSO, int>();
        FruitTypeSO fruitTypeSO = Resources.Load<FruitTypeSO>(typeof(FruitTypeSO).Name);
        foreach (FruitSO fruit in fruitTypeSO.FruitList)
        {
            //Oyun baþlarken oluþturulan meyvelerin sayýlarýný 0 yapýyoruz.
            _fruitPieceAmount[fruit] = 0;
        }
    }

    public int GetFruitAmount(FruitSO fruit)
    {
        return _fruitPieceAmount[fruit];
    }

    public void SetFruitAmount(FruitSO fruit)
    {
        _fruitPieceAmount[fruit] += 1;
    }
}
