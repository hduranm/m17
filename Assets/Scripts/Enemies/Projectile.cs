using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHurtbox>() != null)
        {
            Destroy(gameObject);
        }
    }
}
