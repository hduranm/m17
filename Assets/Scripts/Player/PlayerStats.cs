using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

public class PlayerStats : MonoBehaviour, IShowProgressBar, IDamageable
{
    [SerializeField] private float MaxHealth;
    public float maxHealth => MaxHealth;
    [SerializeField] private float CurrentHealth;
    public float currentHealth => CurrentHealth;
    private bool isDead = false;
    private float immunityDuration = 2f;
    private bool isImmune;
    public event Action<float, float> OnValueChanged;
    [SerializeField] private Hitbox hitBox;
    private WaveController waveController;

    void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    void Start()
    {
        OnValueChanged?.Invoke(currentHealth, maxHealth);
    }

    IEnumerator immunity()
    {
        isImmune = true;
        yield return new WaitForSeconds(immunityDuration);
        isImmune = false;
    }

    public void Kill()
    {
        isDead = true;
        Debug.Log("Personaje ha muerto");

        int currentWave = waveController != null ? waveController.CurrentWaveNumber : 0;
        PlayerPrefs.SetInt("LastWave", currentWave);
        // animator.SetTrigger("Morir");
        StartCoroutine(LoadGameOver());
    }

    private IEnumerator LoadGameOver()
    {
        yield return new WaitForSeconds(0.4f);

        SceneManager.LoadScene("GameOver");

        yield return new WaitForSeconds(0.1f);

        GameOverDisplay gameOverDisplay = FindAnyObjectByType<GameOverDisplay>();
        if (gameOverDisplay != null)
        {
            gameOverDisplay.SetWave(waveController.CurrentWaveNumber);
        }
    }



    public void OnDamage(float damage)
    {
        if (!isImmune && !isDead)
        {
            CurrentHealth -= damage;
            StartCoroutine(immunity());

            OnValueChanged?.Invoke(currentHealth, maxHealth);

            if (CurrentHealth <= 0)
            {
                Kill();
            }
        }
    }
}
