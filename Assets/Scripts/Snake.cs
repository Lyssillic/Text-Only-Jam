using UnityEngine;
using System.Collections.Generic;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<SnakeSegment> _segments = new List<SnakeSegment>();
    private SnakeSegment _head;
    public int initialSize = 4;
    public SnakeSegment segmentPrefab;

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
        ResetState();
    }

    private void Update()
    {
        if (_head.direction.x != 0.0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                _head.SetDirection(Vector2.up, Vector2.zero);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                _head.SetDirection(Vector2.down, Vector2.zero);
            }
        }
        else if (_head.direction.y != 0.0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                _head.SetDirection(Vector2.right, Vector2.zero);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _head.SetDirection(Vector2.left, Vector2.zero);
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].Follow(_segments[i - 1], i, _segments.Count);
        }
        _head.transform.position = new Vector3(
            Mathf.Round(_head.transform.position.x) + _head.direction.x,
            Mathf.Round(_head.transform.position.y) + _head.direction.y);
    }

    public void Grow()
    {
        SnakeSegment segment = Instantiate(this.segmentPrefab);
        segment.Follow(_segments[_segments.Count - 1], _segments.Count, _segments.Count + 1);
        _segments.Add(segment);
    }

    public void ResetState()
    {
        _head.SetDirection(Vector2.right, Vector2.zero);
        _head.transform.position = Vector3.zero;

        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(_head);

        for (int i = 0; i < this.initialSize - 1; i++)
        {
            Grow();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            Grow();
        }
        else if (collision.tag == "Obstacle")
        {
            ResetState();
        }
    }
}
