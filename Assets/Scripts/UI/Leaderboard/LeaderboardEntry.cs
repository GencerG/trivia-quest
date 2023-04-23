using TMPro;
using UnityEngine;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text _score;
    [SerializeField] private TMP_Text _nickname;
    [SerializeField] private TMP_Text _rank;

    public void Initialize(LeaderboardPlayerData data)
    {
        _score.text = data.rank.ToString();
        _nickname.text = data.nickname.ToString();
        _rank.text = data.score.ToString();
    }
}
