using UnityEngine;

public class BasicUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private bool _startActive;

    public void Awake()
    {
        if (!_startActive)
        {
            SetInActive();
        }
        else
        {
            SetActive();
        }    
    }

    public void SetActive() => _panel.SetActive(true);

    public void SetInActive() => _panel.SetActive(false);

}