using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // References
    Rigidbody2D _rb;
    // Class members
    Vector2 _dir;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _rb.velocity = _dir * GameStateEventRelayer.Instance.CurrentSettings.PadSpd;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameTags.Wall))
        {
            _dir = Vector2.zero;
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        _dir = ctx.ReadValue<Vector2>();
    }

}
