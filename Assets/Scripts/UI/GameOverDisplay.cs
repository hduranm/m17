using UnityEngine;
using TMPro;

public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;

    public void SetWave(int wave)
    {
        waveText.text = $"Oleadas sobrevividas: {wave}";
    }
}
