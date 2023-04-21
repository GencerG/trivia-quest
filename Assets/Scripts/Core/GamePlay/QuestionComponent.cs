using TMPro;
using UnityEngine;

public class QuestionComponent : MonoBehaviour
{
    [SerializeField] private TextMeshPro _choiceText;

    public void UpdateText(string choiceText)
    {
        _choiceText.text = choiceText;
    }
}
