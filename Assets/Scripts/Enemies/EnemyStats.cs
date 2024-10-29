using UnityEngine;
using System;

public class EnemyStats : MonoBehaviour, IPoolable, IShowProgressBar, IDamageable
{
    [SerializeField] private float MaxHealth;
    public float maxHealth => MaxHealth;
    [SerializeField] private float CurrentHealth;
    public float currentHealth => CurrentHealth;
    [SerializeField] private float Damage;
    public float damage => Damage;
    public event Action<GameObject> OnReturn;
    public event Action<float, float> OnValueChanged;
    public event Action<GameObject> OnDeath;

    void Start()
    {
        CurrentHealth = MaxHealth;
        OnValueChanged?.Invoke(currentHealth, maxHealth);
    }

    public void Initialize(float maxHealth, float damage)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        Damage = damage;
        OnValueChanged?.Invoke(currentHealth, maxHealth);
    }

    public void Kill()
    {
        OnDeath?.Invoke(gameObject);
        gameObject.SetActive(false);
        OnReturn?.Invoke(gameObject);
    }

    public void OnDamage(float damage)
    {
        CurrentHealth -= damage;

        OnValueChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Kill();
        }
    }
}
