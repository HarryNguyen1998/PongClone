using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] InputReader _inputReader;

    Rigidbody2D _rb;

    GameManager _gameState;


    Vector2 _dir;

    [SerializeField] Vector2 _normal;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _gameState = GameManager.Instance;
    }

    void OnEnable()
    {
        _inputReader.MoveEvent += Move;
        _inputReader.EnableGameplayInput();
    }

    void OnDisable()
    {
        _inputReader.MoveEvent -= Move;
        _inputReader.DisableGameplayInput();
    }

    void FixedUpdate()
    {
        _rb.velocity = _dir * _gameState.CurrentSettings.PadSpd;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameTags.Wall))
        {
            _dir = Vector2.zero;
        }
    }

    void Move(Vector2 dir)
    {
        _dir = dir;
    }

}
