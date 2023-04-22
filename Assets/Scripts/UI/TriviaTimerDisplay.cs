using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TriviaTimerDisplay : MonoBehaviour, IUpdateablePerSecond, ITimerListener
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Image _backgroundImage;

    private int _duration;

    public void OnTimerStart(int duration)
    {
        _duration = duration;
        _timerText.text = duration.ToString();
        _backgroundImage.fillAmount = 1f;
    }

    public void OnUpdate(int secondsLeft)
    {
        _timerText.text = secondsLeft.ToString();
        _backgroundImage.fillAmount = (float)secondsLeft / (float)(_duration);
    }

    public void OnTimerStop()
    {
    }

    public void OnTimeOut()
    {
    }
}
