using TMPro;
using UnityEngine;

public class ScoreUI : BasicUI
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    public void Start()
    {
        ScoreEvent.Instance.OnScore += SetScore;
    }

    private void SetScore(int amount)
    {
        _scoreText.text = "Score : " + amount.ToString();
    }
}
