using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    // References
    [SerializeField] GameSettings _gameSettingsSO;
    [SerializeField] AudioPlayer _audioPlayer;
    Rigidbody2D _rb;
    Transform _tf;

    // Members
    [SerializeField] float _addedAmount = 5.0f;
    [SerializeField] float _roundStartSpd = 9.0f;
    Vector2 _startPos;

    Vector2 _dir;
    public Vector2 Dir { get { return _dir; } }

    float _currentSpd;
    float _time;
    bool _isResetting;
    WaitForSeconds _waitTimeBetweenRound;

    // Vars used to handle when ball is stuck
    Vector2 _posPrevFrame;
    int _stuckCounter;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tf = GetComponent<Transform>();
    }

    void Start()
    {
        _startPos = _tf.position;

        // If vec is too horizontal, ball will ping pong between pads, which isn't fun. So, add more angles.
        _dir = new Vector2(Mathf.Sign(Random.Range(-1.0f, 1.0f)), Random.Range(-1.0f, 1.0f));
        if (_dir.y < 0.3f && _dir.y > 0.0f)
            _dir.y += 0.3f;
        else if (_dir.y > -0.3f && _dir.y <= 0.0f)
            _dir.y -= 0.3f;
        _dir.Normalize();

        _currentSpd = _roundStartSpd;
        _waitTimeBetweenRound = new WaitForSeconds(_gameSettingsSO.WaitTimeBetweenRound);
    }

    void OnEnable()
    {
        GameManager.Instance.RoundWasOver += Reset;
    }

    void OnDestroy()
    {
        GameManager.Instance.RoundWasOver -= Reset;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameTags.Pad))
        {
            _currentSpd = GameManager.Instance.CurrentSettings.BallSpd;
            _dir = Vector2.Reflect(_dir, collision.GetContact(0).normal);

            // Every 7s increases ball speed by _addedAmount
            _currentSpd += _addedAmount * (int)(_time / 7);

            _audioPlayer.PlayBallHitPadClip();
        }
        else if (collision.collider.CompareTag(GameTags.Wall))
        {
            _dir = Vector2.Reflect(_dir, collision.GetContact(0).normal);
        }

    }

    void FixedUpdate()
    {
        if (Application.isEditor)
            Debug.DrawLine(_posPrevFrame, _posPrevFrame + _dir);

        if (_isResetting)
            return;

        _rb.velocity = _dir * _currentSpd;

        // If ball is stuck after fixed frames, teleport it a bit
        if (Mathf.Approximately(_posPrevFrame.x, _tf.position.x) &&
            Mathf.Approximately(_posPrevFrame.y, _tf.position.y))
        {
            ++_stuckCounter;
            if (_stuckCounter >= 50)
            {
                _posPrevFrame.x += 0.1f;
                _tf.position = _posPrevFrame;
            }
        }
        else
        {
            _posPrevFrame = _tf.position;
            _stuckCounter = 0;
        }

        _time += Time.deltaTime;
    }

    public void Reset(bool leftWon)
    {
        _tf.position = _startPos;

        // If vec is too horizontal, ball will ping pong between pads, which isn't fun. So, add more angles.
        _dir = new Vector2(1.0f, Random.Range(-1.0f, 1.0f));
        if (_dir.y < 0.3f && _dir.y > 0.0f)
            _dir.y += 0.3f;
        else if (_dir.y > -0.3f && _dir.y <= 0.0f)
            _dir.y -= 0.3f;
        _dir.Normalize();

        if (!leftWon)
            _dir.x *= -1.0f;

        _currentSpd = _roundStartSpd;
        _time = 0.0f;
        _rb.velocity = Vector2.zero;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<TrailRenderer>().enabled = false;
        _stuckCounter = 0;
        _isResetting = true;

        if (GameManager.Instance.CurrentState != GameState.kGameOverMenu)
            StartCoroutine(CO_Reset(leftWon));
    }

    public IEnumerator CO_Reset(bool leftWon)
    {
        yield return _waitTimeBetweenRound;
        _isResetting = false;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<TrailRenderer>().enabled = true;
    }

}
