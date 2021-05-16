using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the movement of the snake and growing in size.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    /// <summary>
    /// The list of segments of the snake.
    /// </summary>
    private List<SnakeSegment> _segments = new List<SnakeSegment>();

    /// <summary>
    /// The head snake segment.
    /// </summary>
    private SnakeSegment _head;

    /// <summary>
    /// The object that is cloned when creating a new segment to grow the snake.
    /// </summary>
    [Tooltip("The object that is cloned when creating a new segment to grow the snake.")]
    public SnakeSegment segmentPrefab;

    /// <summary>
    /// The number of segments the snake has initially.
    /// </summary>
    [Tooltip("The number of segments the snake has initially.")]
    public int initialSize = 4;

    /// <summary>
    /// The number of lives the snake has lost.
    /// </summary>
    int livesLost = 0;

    /// <summary>
    /// The lives object that holds the snake life sprites.
    /// </summary>
    public List<GameObject> livesObj;

    public GameObject fruitObj;
    FruitCollected fruitScript;

    AudioSource audioSource;
    AudioClip snake;
    AudioClip whoosh;
    AudioClip crunch;

    private void Awake()
    {
        _head = GetComponent<SnakeSegment>();

        if (_head == null)
        {
            _head = this.gameObject.AddComponent<SnakeSegment>();
            _head.hideFlags = HideFlags.HideInInspector;
        }
    }

    private void Start()
    {
        GameObject fruitObj = GameObject.Find("Fruit Collected");
        fruitScript = fruitObj.GetComponent<FruitCollected>();
        fruitScript.fruitCollected = 0;
        livesLost = 0;
        ResetState(false);

        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        snake = (AudioClip)Resources.Load("SFX/Snake", typeof(AudioClip));
        whoosh = (AudioClip)Resources.Load("SFX/Whoosh", typeof(AudioClip));
        crunch = (AudioClip)Resources.Load("SFX/Crunch", typeof(AudioClip));
    }

    private void Update()
    {
        // If moving horizontal, then only allow turning up or down
        if (_head.direction.x != 0.0f)
        {
            // Set the direction based on the input key being pressed
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                audioSource.PlayOneShot(whoosh, .5f);
                _head.SetDirection(Vector2.up, Vector2.zero);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                audioSource.PlayOneShot(whoosh, .5f);
                _head.SetDirection(Vector2.down, Vector2.zero);
            }
        }
        // If moving vertical, then only allow turning left or right
        else if (_head.direction.y != 0.0f)
        {
            // Set the direction based on the input key being pressed
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                audioSource.PlayOneShot(whoosh, .5f);
                _head.SetDirection(Vector2.right, Vector2.zero);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                audioSource.PlayOneShot(whoosh, .5f);
                _head.SetDirection(Vector2.left, Vector2.zero);
            }
        }
    }

    private void FixedUpdate()
    {
        // Have each segment follow the one in front of it. We must do this in
        // reverse order so the position is set to the previous position,
        // otherwise they will all be stacked on top of each other.
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].Follow(_segments[i - 1], i, _segments.Count);
        }

        // Increase the snake's position by one in the direction they are
        // moving. Round the position to ensure it stays aligned to the grid.
        _head.transform.position = new Vector3(
            Mathf.Round(_head.transform.position.x) + _head.direction.x,
            Mathf.Round(_head.transform.position.y) + _head.direction.y);
    }

    public void Grow()
    {
        // Create a new segment and have it follow the last segment
        SnakeSegment segment = Instantiate(this.segmentPrefab);
        segment.Follow(_segments[_segments.Count - 1], _segments.Count, _segments.Count + 1);

        // Add the segment to the end of the list
        _segments.Add(segment);
    }

    public void ResetState(bool lose)
    {
        // Check livesLost, game over if 3
        if (livesLost == 3)
        {
            SceneManager.LoadScene("End");
        }
        else if (lose)
        {
            // Remove a life
            RemoveLife();
        }


        // Set the initial direction of the snake, starting at the origin
        // (center of the grid)
        _head.SetDirection(Vector2.right, Vector2.zero);
        _head.transform.position = Vector3.zero;

        // Start at 1 to skip destroying the head
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        // Clear the list then add the head as the first segment
        _segments.Clear();
        _segments.Add(_head);

        // Grow the snake to the initial size -1 since the head was already
        // added
        for (int i = 0; i < this.initialSize - 1; i++)
        {
            Grow();
        }
    }

    public void RemoveLife()
    {
        livesObj[livesLost].SetActive(false);
        livesLost++;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            audioSource.PlayOneShot(crunch, .5f);

            // Food eaten, increase the size of the snake
            Grow();

            // Add fruit to collected
            fruitScript.fruitCollected++;
            fruitScript.updateFruitCount(fruitObj, true);
        }
        else if (other.tag == "Obstacle")
        {
            // Game over, reset the state of the snake
            audioSource.PlayOneShot(snake, .5f);
            ResetState(true);
        }
    }

}
