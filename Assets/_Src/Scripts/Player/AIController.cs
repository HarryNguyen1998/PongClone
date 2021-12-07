using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIController : MonoBehaviour
{
    public float ballYPos;
    public float CurrentYPos;

    // Referernces
    Transform _ballTransform;
    Rigidbody2D _rb;
    GameManager _gameState;

    // Class members
    Vector2 _dir;

    void Awake()
    {
        _ballTransform = GameObject.FindWithTag(GameTags.Ball).GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _gameState = GameManager.Instance;
    }

    void FixedUpdate()
    {
        if (ballYPos > (CurrentYPos + 0.3f))
            _dir = Vector2.up;
        else if (ballYPos < (CurrentYPos - 0.3f))
            _dir = Vector2.down;
        else
            _dir = Vector2.zero;
        _rb.velocity = Vector2.Lerp(_rb.velocity, _dir * _gameState.CurrentSettings.PadSpd, 0.15f);
        ballYPos = _ballTransform.position.y;
        CurrentYPos = transform.position.y;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameTags.Wall))
        {
            _dir = Vector2.zero;
        }
    }

    public void Reset()
    {
        _dir = Vector2.zero;
    }

}
