using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WaveController : MonoBehaviour
{
    [SerializeField] private WaveScriptableObject[] waves;
    [SerializeField] private Transform[] spawnPoints;
    private int waveNumber = 0;
    private List<GameObject> currentEnemies = new List<GameObject>();
    public event System.Action<int> OnWaveStarted;
    public int CurrentWaveNumber => waveNumber + 1;

    private void Start()
    {
        StartWave();
    }

    private void StartWave()
    {
        if (waveNumber >= waves.Length)
        {
            SceneManager.LoadScene("GameWin");
        }
        

        OnWaveStarted?.Invoke(waveNumber + 1);

        WaveScriptableObject currentWave = waves[waveNumber];

        for (int i = 0; i < currentWave.melee; i++)
        {
            if (Random.value < currentWave.meleeProbability && currentWave.enemies.Length > 0)
            {
                SpawnEnemy(currentWave.enemies[0]);
            }
        }

        for (int i = 0; i < currentWave.ranged; i++)
        {
            if (Random.value < currentWave.rangedProbability && currentWave.enemies.Length > 1)
            {
                SpawnEnemy(currentWave.enemies[1]);
            }
        }

    }

    private void SpawnEnemy(EnemyScriptableObject enemySO)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemySO.prefab, spawnPoint.position, Quaternion.identity);
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        enemyStats.Initialize(enemySO.maxHealth, enemySO.damage);
        currentEnemies.Add(enemy);

        enemy.GetComponent<EnemyStats>().OnDeath += EnemyDestroyed;
    }

    private void EnemyDestroyed(GameObject enemy)
{
    currentEnemies.Remove(enemy);

    if (currentEnemies.Count == 0)
    {
        if (waveNumber >= waves.Length - 1) 
        {
            PlayerPrefs.SetInt("LastWave", waveNumber + 1);
            PlayerPrefs.Save(); 
            SceneManager.LoadScene("GameWin");
        }
        else
        {
            waveNumber++;
            StartWave();
        }
    }
}

}
