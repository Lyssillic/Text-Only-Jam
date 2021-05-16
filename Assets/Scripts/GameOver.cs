using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject fruitObj;

    private void Start()
    {
        GameObject fruitObj = GameObject.Find("Fruit Collected");
        FruitCollected fruitScript = fruitObj.GetComponent<FruitCollected>();
        fruitScript.updateFruitCount(GameObject.Find("fruit count"), false);
    }
}
