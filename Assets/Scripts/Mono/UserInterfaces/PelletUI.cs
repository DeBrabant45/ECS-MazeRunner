using TMPro;
using UnityEngine;

public class PelletUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pelletText;

    public void Start()
    {
        PelletEvent.Instance.OnPelletChange += SetPellets;
    }

    private void SetPellets(int amount)
    {
        _pelletText.text = "Pellets : " + amount.ToString();
    }
}
