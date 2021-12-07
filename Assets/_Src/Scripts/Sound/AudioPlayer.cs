using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Contents")]
    // References
    [SerializeField] AudioClip _ballHitPadClip;
    [SerializeField] AudioClip _roundOverLeft;
    [SerializeField] AudioClip _roundOverRight;

    // Members
    [SerializeField, Range(0.0f, 1.0f)] float _ballHitPadVol;
    [SerializeField, Range(0.0f, 1.0f)] float _roundOverVol;

    private void OnEnable()
    {
        GameManager.Instance.RoundWasOver += PlayClip;
    }

    private void OnDisable()
    {
        GameManager.Instance.RoundWasOver -= PlayClip;
    }

    public void PlayBallHitPadClip()
    {
        AudioSource.PlayClipAtPoint(_ballHitPadClip, Camera.main.transform.position, _ballHitPadVol);
    }

    void PlayClip(bool leftWon)
    {
        if (leftWon)
            AudioSource.PlayClipAtPoint(_roundOverLeft, Camera.main.transform.position, _roundOverVol);
        else
            AudioSource.PlayClipAtPoint(_roundOverRight, Camera.main.transform.position, _roundOverVol);

    }
}
