using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameplayDialog : MonoBehaviour
{
    // References
    [SerializeField] TMP_Text _leftPadWon;
    [SerializeField] TMP_Text _rightPadWon;
    [SerializeField] TMP_Text _leftPadScore;
    [SerializeField] TMP_Text _rightPadScore;
    [SerializeField] TMP_Text _roundCnt;
    [SerializeField] Image _dividerLine;
    [SerializeField] Image _borderFilled;
    [SerializeField] Slider _borderSlider;

    [SerializeField] GameSettings _animationSettings;

    // Members
    Color _dividerLineOriginalColor;

    void OnEnable()
    {
        GameManager.Instance.RoundWasOver += DisplayPlayerWon;
        SettingsDialog.GameSettingsChanged += ShowRoundCnt;
    }

    void OnDisable()
    {
        SettingsDialog.GameSettingsChanged -= ShowRoundCnt;
        GameManager.Instance.RoundWasOver -= DisplayPlayerWon;
    }

    void Start()
    {
        _leftPadWon.gameObject.SetActive(false);
        _rightPadWon.gameObject.SetActive(false);
        _leftPadScore.text = "0";
        _rightPadScore.text = "0";
        ShowRoundCnt();

    }

    public void ShowRoundCnt()
    {
        if (GameManager.Instance.IsInGame)
            _roundCnt.text = "ROUNDS\n" + GameManager.Instance.CurrentSettings.RoundCnt.ToString();
        else
            _roundCnt.text = "ROUNDS\n???";
    }

    public void DisplayPlayerWon(bool leftWon)
    {
        if (leftWon)
        {
            if (GameManager.Instance.CurrentState == GameState.kGameOverMenu)
            {
                _leftPadWon.text = "VICTORY";
                _rightPadWon.text = "DEFEATED";
                _rightPadWon.gameObject.SetActive(true);
            }

            StartCoroutine(Animate(_leftPadWon, _leftPadScore, GameManager.Instance.ScoreLeft,
                Slider.Direction.LeftToRight, Slider.Direction.RightToLeft, _animationSettings.LeftPadMat.color));
        }
        else
        {
            if (GameManager.Instance.CurrentState == GameState.kGameOverMenu)
            {
                _leftPadWon.text = "DEFEATED";
                _rightPadWon.text = "VICTORY";
                _leftPadWon.gameObject.SetActive(true);
            }

            StartCoroutine(Animate(_rightPadWon, _rightPadScore, GameManager.Instance.ScoreRight,
                Slider.Direction.RightToLeft, Slider.Direction.LeftToRight, _animationSettings.RightPadMat.color));
        }
    }

    /// <summary>
    /// Animate gameplay widgets: fade in and out scoreText, wonText, divider line, and also fill then unfill in a direction.
    /// </summary>
    /// <param name="score">The score to display on <c>scoreText</c></param>
    /// <param name="enterDir">The direction to start filling border</param>
    /// <param name="exitDir">Opposite direction of <c>enterDir</c>, to unfill the border</param>
    /// <param name="color">Color of <c>_dividerLine</c> and <c>_borderFilled</c> while animating</param>
    /// <returns></returns>
    IEnumerator Animate(TMP_Text wonText, TMP_Text scoreText, int score, Slider.Direction enterDir, Slider.Direction exitDir, Color color)
    {
        wonText.gameObject.SetActive(true);
        scoreText.text = score.ToString();
        _borderFilled.color = color;

        wonText.DOFade(0.0f, _animationSettings.AnimationCycle).SetEase(Ease.InOutSine).SetLoops(4, LoopType.Yoyo)
            .OnComplete(() =>
        {
            if (GameManager.Instance.CurrentState != GameState.kGameOverMenu)
                wonText.gameObject.SetActive(false);
        });

        scoreText.DOFade(0.0f, _animationSettings.AnimationCycle).SetEase(Ease.InOutSine).SetLoops(4, LoopType.Yoyo);

        _dividerLine.DOColor(color, _animationSettings.AnimationCycle).SetEase(Ease.InOutSine).SetLoops(4, LoopType.Yoyo);

        _borderSlider.SetDirection(enterDir, false);
        _borderSlider.DOValue(1, _animationSettings.AnimationCycle * 2.0f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(_animationSettings.AnimationCycle * 2.0f);
        _borderSlider.SetDirection(exitDir, false);
        _borderSlider.DOValue(0, _animationSettings.AnimationCycle).SetEase(Ease.InOutSine);
    }

}
