using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PadData))]
public class AIController : MonoBehaviour
{
    // Referernces
    PadData _padData;
    Transform _tf;
    Transform _ballTf;
    BallController _ballC;
    Rigidbody2D _rb;

    // Class members
    Vector2 _dir;

    void Awake()
    {
        _padData = GetComponent<PadData>();
        _tf = transform;
        _ballTf = GameObject.FindWithTag(GameTags.Ball).GetComponent<Transform>();
        _ballC = _ballTf.GetComponent<BallController>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Only move if the ball is moving towards you
        if (Vector2.Dot(_padData.Normal, _ballC.Dir) > 0.0f)
            _dir = Vector2.zero;
        else
        {
            // @note Perhaps may add noise for speed, or reaction time to make it more natural?
            if (_ballTf.position.y > (_tf.position.y + 0.3f))
                _dir = Vector2.up;
            else if (_ballTf.position.y < (_tf.position.y - 0.3f))
                _dir = Vector2.down;
            else
                _dir = Vector2.zero;
        }

        _rb.velocity = Vector2.Lerp(_rb.velocity, _dir * GameStateEventRelayer.Instance.CurrentSettings.PadSpd, 0.1f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameTags.Wall))
        {
            _dir = Vector2.zero;
        }
    }

}
