using UnityEngine;

[CreateAssetMenu(fileName = "NuevaOleada", menuName = "ScriptableObjects/Oleada")]
public class WaveScriptableObject : ScriptableObject
{
    public EnemyScriptableObject[] enemies; 
    public int melee; 
    public int ranged;
    public float meleeProbability;
    public float rangedProbability; 
}
