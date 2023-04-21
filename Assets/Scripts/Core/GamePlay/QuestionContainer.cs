using UnityEngine;

public class QuestionContainer : MonoBehaviour
{
    [SerializeField] private QuestionComponent _question;

    public void Initialize()
    {
        _question.UpdateText("");
    }
}
