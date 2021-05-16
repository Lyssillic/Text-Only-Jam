using UnityEngine;
using UnityEngine.SceneManagement;

public class FruitCollected : MonoBehaviour
{
    public int fruitCollected = 0;
    bool destroyed = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (!destroyed)
        {
            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "End")
            {
                Destroy(gameObject);
            }
        }
    }

    public void updateFruitCount(GameObject fruitObj, bool game)
    {
        string fruit = fruitCollected.ToString();

        if (fruit.Length == 1)
        {
            fruitObj.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Sprites/text/numbers/" + fruit, typeof(Sprite));
        }
        else
        {
            fruitObj.GetComponent<SpriteRenderer>().sprite = null;
            var dist = 0.0f;

            if (game)
            {
                foreach (Transform child in fruitObj.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }

                for (int i = 0; i < fruit.Length; i++)
                {
                    dist = dist - .5f;
                    var prefab = (GameObject)Resources.Load("Prefabs/gameCount");
                    var numSprite = Instantiate(prefab, fruitObj.transform.position - new Vector3(dist, 0, 0), fruitObj.transform.rotation);
                    numSprite.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Sprites/text/numbers/" + fruit[i], typeof(Sprite));
                    numSprite.transform.parent = fruitObj.transform;
                }
            }
            else
            {
                for (int i = 0; i < fruit.Length; i++)
                {
                    dist = dist - 1f;
                    var prefab = (GameObject)Resources.Load("Prefabs/endCount");
                    var numSprite = Instantiate(prefab, fruitObj.transform.position - new Vector3(dist, 0, 0), fruitObj.transform.rotation);
                    numSprite.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Sprites/text/numbers/" + fruit[i], typeof(Sprite));
                    numSprite.transform.parent = fruitObj.transform;
                }
            }
        }
    }
}
