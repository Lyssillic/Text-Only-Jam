using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCollected : MonoBehaviour
{
    static public int fruitCollected = 0;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
