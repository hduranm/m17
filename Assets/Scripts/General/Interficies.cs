using System;

public interface IShowProgressBar
{
    public event Action<float, float> OnValueChanged;
}

public interface IDamageable{
    public void OnDamage(float damage);
}