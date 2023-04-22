using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriviaCountDownTimer : MonoBehaviour
{
    private List<IUpdateablePerSecond> _updateables;
    private List<ITimerListener> _listeners;
    private Coroutine _countDownRoutine;
    private readonly YieldInstruction _oneSecond = new WaitForSeconds(1f);
    private TriviaQuestController _controller;

    private int _secondsLeft;
    private int _duration;

    public void Initialize(TriviaQuestController controller)
    {
        _controller = controller;
        _updateables = new List<IUpdateablePerSecond>();
        _listeners = new List<ITimerListener>();
        _duration = ScopeManager.Instance.GetService<GameStrategyService>(Scope.GAMEPLAY).GetStageDuration();
        _secondsLeft = _duration;
    }

    public void Clear()
    {
        _updateables.Clear();
        _listeners.Clear();
        _countDownRoutine = null;
    }

    public void AddUpdateable(IUpdateablePerSecond updateable)
    {
        _updateables.Add(updateable);
    }

    public void AddListener(ITimerListener listener)
    {
        _listeners.Add(listener);
    }

    public void ResetTimer()
    {
        _secondsLeft = _duration;
    }

    public void StartTimer()
    {
        _countDownRoutine = StartCoroutine(CountDownRoutine());
        StartAll();
    }

    public void StopTimer()
    {
        StopCoroutine(_countDownRoutine);
        _countDownRoutine = null;
        ResetTimer();
    }

    private IEnumerator CountDownRoutine()
    {
        while (true)
        {
            yield return _oneSecond;

            _secondsLeft--;
            UpdateAll(_secondsLeft);

            if (_secondsLeft > 0 )
            {
                continue;
            }

            StopAll();
            yield return _controller.OnTimeOut();

            yield break;
        }
    }

    private void StartAll()
    {
        foreach (var listener in _listeners)
        {
            listener.OnTimerStart(_duration);
        }
    }

    private void UpdateAll(int secondsLeft)
    {
        foreach (var updateable in _updateables)
        {
            updateable.OnUpdate(secondsLeft);
        }
    }

    private void StopAll()
    {
        foreach (var listener in _listeners)
        {
            listener.OnTimerStop();
        }
    }
}
