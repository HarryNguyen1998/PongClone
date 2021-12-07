using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    // References
    [SerializeField] GameSettings _gameSettingsSO;
    Rigidbody2D _rb;
    AudioPlayer _audioPlayer;

    // Members
    public Vector2 Dir;

    [SerializeField] float _addedAmount = 5.0f;
    float _roundStartSpd = 9.0f;
    Vector2 _originalPos;
    float _spd;
    GameManager _gameManager;
    float _time = 0.0f;
    bool _isResetting;

    // Vars used to handle when ball is stuck
    Vector2 _posPrevFrame;
    int _stuckCounter;

    void Awake()
    {
        _gameManager = GameManager.Instance;
        _rb = GetComponent<Rigidbody2D>();
        _audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void Start()
    {
        _originalPos = transform.position;

        // If vec is too horizontal, ball will ping pong between pads, which isn't fun. So, add more angles.
        Dir = new Vector2(1.0f, Random.Range(-1.0f, 1.0f));
        if (Dir.y < 0.3f && Dir.y > 0.0f)
            Dir.y += 0.3f;
        else if (Dir.y > -0.3f && Dir.y <= 0.0f)
            Dir.y -= 0.3f;
        Dir.Normalize();

        // Random which dir it would go 1st.
        if (Mathf.RoundToInt(Random.value) == 0)
            Dir.x *= -1.0f;

        _spd = _roundStartSpd;
    }

    void OnEnable()
    {
        _gameManager.RoundWasOver += Reset;
        _gameManager.GameStateChanged += DisableMovement;
    }

    void OnDestroy()
    {
        _gameManager.GameStateChanged -= DisableMovement;
        _gameManager.RoundWasOver -= Reset;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameTags.Pad))
        {
            _spd = _gameManager.CurrentSettings.BallSpd;
            Dir = Vector2.Reflect(Dir, collision.GetContact(0).normal);

            // Every 7s increases ball speed by _addedAmount
            _spd += _addedAmount * (int)(_time / 7);

            _audioPlayer.PlayBallHitPadClip();
        }
        else if (collision.collider.CompareTag(GameTags.Wall))
        {
            WallZone wallData = collision.collider.GetComponent<WallZone>();
            Dir = Vector2.Reflect(Dir, wallData.Normal);

        }

    }

    void FixedUpdate()
    {
#if UNITY_EDITOR
        Debug.DrawLine(_posPrevFrame, _posPrevFrame + Dir);
#endif
        if (_isResetting)
            return;

        _rb.velocity = Dir * _spd;

        // If ball is stuck after fixed frames, teleport it a bit
        if (Mathf.Approximately(_posPrevFrame.x, transform.position.x) &&
            Mathf.Approximately(_posPrevFrame.y, transform.position.y))
        {
            ++_stuckCounter;
            if (_stuckCounter >= 200)
            {
                _posPrevFrame.x += 0.1f;
                transform.position = _posPrevFrame;
            }
        }
        else
        {
            _posPrevFrame = transform.position;
            _stuckCounter = 0;
        }

        _time += Time.deltaTime;
    }

    public void Reset(bool leftWon)
    {
        StartCoroutine(CO_Reset(leftWon));
    }

    public IEnumerator CO_Reset(bool leftWon)
    {
        GetComponent<Transform>().position = _originalPos;

        // If vec is too horizontal, ball will ping pong between pads, which isn't fun. So, add more angles.
        Dir = new Vector2(1.0f, Random.Range(-1.0f, 1.0f));
        if (Dir.y < 0.3f && Dir.y > 0.0f)
            Dir.y += 0.3f;
        else if (Dir.y > -0.3f && Dir.y <= 0.0f)
            Dir.y -= 0.3f;
        Dir.Normalize();

        if (!leftWon)
            Dir.x *= -1.0f;

        _spd = _roundStartSpd;
        _time = 0.0f;
        _rb.velocity = Vector2.zero;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<TrailRenderer>().enabled = false;
        _stuckCounter = 0;
        _isResetting = true;
        yield return new WaitForSeconds(_gameSettingsSO.WaitTimeBetweenEachRound);
        _isResetting = false;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<TrailRenderer>().enabled = true;
    }

    void DisableMovement(GameState newState)
    {
        if (newState == GameState.kGameOverMenu)
            enabled = false;
    }

}
