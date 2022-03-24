using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] Transform _camTf;
    [Header("Contents")]
    // References
    [SerializeField] AudioClip _ballHitPadClip;
    [SerializeField] AudioClip _roundOverLeft;
    [SerializeField] AudioClip _roundOverRight;

    // Members
    [SerializeField, Range(0.0f, 1.0f)] float _ballHitPadVol;
    [SerializeField, Range(0.0f, 1.0f)] float _roundOverVol;

    void OnEnable()
    {
        GameStateEventRelayer.Instance.RoundWasOver += PlayClip;
    }

    void OnDisable()
    {
        GameStateEventRelayer.Instance.RoundWasOver -= PlayClip;
    }

    public void PlayBallHitPadClip()
    {
        AudioSource.PlayClipAtPoint(_ballHitPadClip, _camTf.position, _ballHitPadVol);
    }

    void PlayClip(bool leftWon)
    {
        if (!GameStateEventRelayer.Instance.IsInGame() &&
            GameStateEventRelayer.Instance.PeekState() != GameState.kGameOver)
            return;

        if (leftWon)
            AudioSource.PlayClipAtPoint(_roundOverLeft, _camTf.position, _roundOverVol);
        else
            AudioSource.PlayClipAtPoint(_roundOverRight, _camTf.position, _roundOverVol);

    }
}
