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
                GameManager.Instance.IncrementScore(false);
            else
                GameManager.Instance.IncrementScore(true);
        }

    }

}
