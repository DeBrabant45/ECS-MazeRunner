using System;
using UnityEngine;

public class ScoreEvent  : MonoBehaviour
{
    public static ScoreEvent Instance;
    public Action<int> OnScore;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void Invoke(int amount)
    {
        OnScore?.Invoke(amount);
    }
}
