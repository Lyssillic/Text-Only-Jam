using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject fruitObj;
    private void Start()
    {
        FruitCollected fruitScript = GameObject.Find("Fruit Collected").GetComponent<FruitCollected>();
        string fruit = fruitScript.fruitCollected.ToString();

        if (fruit.Length == 1)
        {
            fruitObj.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Sprites/text/numbers/" + fruit, typeof(Sprite));
        }
        else
        {
            var dist = 0.0f;
            for (int i = 0; i < fruit.Length; i++)
            {
                dist = dist - 1f;
                var prefab = (GameObject)Resources.Load("Prefabs/count");
                var numSprite = Instantiate(prefab, fruitObj.transform.position - new Vector3(dist, 0, 0), fruitObj.transform.rotation);
                numSprite.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Sprites/text/numbers/" + fruit[i], typeof(Sprite));
                numSprite.transform.parent = fruitObj.transform;
            }
        }
    }
}
