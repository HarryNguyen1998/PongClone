using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] InputReader _inputReader;

    Rigidbody2D _rb;

    Vector2 _dir;

    [SerializeField] Vector2 _normal;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

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
        _rb.velocity = _dir * GameManager.Instance.CurrentSettings.PadSpd;
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
