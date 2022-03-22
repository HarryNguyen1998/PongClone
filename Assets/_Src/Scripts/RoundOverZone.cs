using System.Collections;
using UnityEngine;

public class RoundOverZone : MonoBehaviour
{
    public bool IsLeftZone;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameTags.Ball))
        {
            if (IsLeftZone)
                GameStateEventRelayer.Instance.IncrementScore(false);
            else
                GameStateEventRelayer.Instance.IncrementScore(true);
        }

    }

}
