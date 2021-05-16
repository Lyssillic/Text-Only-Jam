using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public List<Sprite> fruitSprites;

    private void Start()
    {
        RandomizePosition();
        RandomizeFruit();
    }

    private void RandomizePosition()
    {
        Bounds bounds = this.gridArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }

    private void RandomizeFruit()
    {
        int randomIndex = Random.Range(0, fruitSprites.Count);
        Sprite randomFruit = fruitSprites[randomIndex];
        GetComponent<SpriteRenderer>().sprite = randomFruit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            RandomizePosition();
            RandomizeFruit();
        }
    }
}
