using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnityEvent _onWin;
    [SerializeField] private UnityEvent _onLose;
    private int _score;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    private void Start()
    {
        ScoreEvent.Instance.Invoke(_score);
    }

    public void Reset()
    {
        _score = 0;
    }

    public void Win()
    {
        _onWin.Invoke();
    }

    public void Lose()
    {
        _onLose.Invoke();
    }

    public void AddScore(int amount)
    {
        _score += amount;
        ScoreEvent.Instance.Invoke(_score);
    }
}
