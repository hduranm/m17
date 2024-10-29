using UnityEngine;
using TMPro; 

public class CurrentWave : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText; 
    [SerializeField] private WaveController waveController; 

    private void OnEnable()
    {
        waveController.OnWaveStarted += UpdateWaveText;
    }

    private void OnDisable()
    {
        waveController.OnWaveStarted -= UpdateWaveText;
    }

    private void UpdateWaveText(int waveNumber)
    {
        waveText.text = $"WAVE {waveNumber}";
    }
}
