using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnityEvent _onWin;
    [SerializeField] private UnityEvent _onLose;
    [SerializeField] private int _liveCount;
    private int _score;
    private LevelManager _levelManager;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        _levelManager = FindObjectOfType<LevelManager>();
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

    public void LoseLife()
    {
        _liveCount--;
        if (_liveCount <= 0)
        {
            Lose();
        }
    }

    public void Lose()
    {
        _onLose.Invoke();
        _levelManager.Load(0);
    }

    public void AddScore(int amount)
    {
        _score += amount;
        ScoreEvent.Instance.Invoke(_score);
    }

    public void SetPelletCount(int amount)
    {
        PelletEvent.Instance.Invoke(amount);
        if (amount <= 0)
        {
            Win();
        }
    }
}
