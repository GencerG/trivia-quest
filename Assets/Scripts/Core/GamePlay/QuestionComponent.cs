using TMPro;
using UnityEngine;

public abstract class QuestionComponent : MonoBehaviour
{
    [SerializeField] private TextMeshPro _componentText;

    public void UpdateText(string componentText)
    {
        _componentText.text = componentText;
    }

    public virtual void PlayInAnimation()
    {

    }

    public virtual void PlayOutAnimation()
    {

    }
}
