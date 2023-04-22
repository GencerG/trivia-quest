using UnityEngine;

public class GamePlayInputManager
{
    private const string SPRITE_BUTTON_TAG = "SpriteButton";

    private InputListener _inputListener;
    private SpriteButton _clickedSpriteButton;

    public GamePlayInputManager()
    {
        _inputListener = ScopeManager.Instance.GetService<InputService>(Scope.GAMEPLAY).InputListener;

        _inputListener.TouchStarted += TouchStarted;
        _inputListener.TouchEnded += TouchEnded;
    }

    public void TouchStarted(Vector3 touchPosition)
    {
        var hitCollider = Physics2D.OverlapPoint(touchPosition);
        if (hitCollider == null || !hitCollider.CompareTag(SPRITE_BUTTON_TAG))
        {
            return;
        }

        if (!hitCollider.TryGetComponent<SpriteButton>(out var spireButton))
        {
            _clickedSpriteButton = null;
            return;
        }

        _clickedSpriteButton = spireButton;
        _clickedSpriteButton.ButtonDown();
    }

    public void TouchEnded(Vector3 touchPosition)
    {
        if (_clickedSpriteButton == null) return;

        _clickedSpriteButton.ButtonUp();
        _clickedSpriteButton = null;
    }

    public void Destroy()
    {
        _inputListener.TouchStarted += TouchStarted;
        _inputListener.TouchEnded += TouchEnded;
    }
}
