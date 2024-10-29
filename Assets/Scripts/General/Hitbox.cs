using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private float damage;

    public float Damage
    {
        set => damage = value;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<IDamageable>(out IDamageable other))
            other.OnDamage(damage);
    }
}
