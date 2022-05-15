using System;
using UnityEngine;

public class PelletEvent  : MonoBehaviour
{
    public static PelletEvent Instance;
    public Action<int> OnPelletChange;

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
        OnPelletChange?.Invoke(amount);
    }
}
