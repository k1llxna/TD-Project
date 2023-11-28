using UnityEngine;
using System.Collections; // coroutine included
using UnityEngine.UI; // Text


public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;

    public Wave[] waves;
    public int EnemiesAlive = 0;
    public Transform spawnPoint;

    public float waveIntermission = 5.5f;
    public Text waveCountText;

    private float countDown = 2f;

    private int waveIndex;

    void Update()
    {

        if (EnemiesAlive < 0)
        {
            return;
        }

        if (waveIndex == waves.Length)
        {
            this.enabled = false;
        }

        if (countDown <= 0f)
        {
            StartCoroutine(SpawnWave()); // timer
            countDown = waveIntermission;
            return;
        }
        countDown -= Time.deltaTime;

        countDown = Mathf.Clamp(countDown, 0f, Mathf.Infinity);

        waveCountText.text = string.Format("{0:00.0}", countDown);
    }

    IEnumerator SpawnWave()
    {
        PlayerStats.Rounds++;

        Wave wave = waves[waveIndex];

        EnemiesAlive = wave.count;

        Debug.Log("Wave incoming");
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }
        waveIndex++;
    }

    void SpawnEnemy(GameObject enemy)
    {
        GameObject e = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        Transform parent = GameObject.Find("Enemies").transform;

        e.transform.parent = parent;
    }
}
