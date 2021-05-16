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
}
