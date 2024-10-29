using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public GameObject prefab;
    public float maxHealth;
    public float damage;
    public float speed; 
    public Color color;
}
