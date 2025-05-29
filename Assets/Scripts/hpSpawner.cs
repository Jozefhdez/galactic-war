using UnityEngine;

public class hpSpawner : MonoBehaviour
{
    public GameObject hpPrefab;
    public shipScript isAlive;
    public bool isAliveVar;
    private int lastHpSpawnScore = 0;

    void Update()
    {
        isAliveVar = isAlive.isAliveFunction();
        if (!isAliveVar)
            return;

        LogicScript logic = GameObject.FindWithTag("Logic").GetComponent<LogicScript>();
        int playerScore = logic.playerScore;

        // Solo spawnea si han pasado 10 puntos desde el último spawn
        if (playerScore - lastHpSpawnScore >= 10)
        {
            spawnHp();
            lastHpSpawnScore = playerScore;
        }
    }

    void spawnHp()
    {
        float minDistance = 3.0f;
        GameObject player = GameObject.FindWithTag("ship");
        if (player == null) return;
        Vector2 playerPos = player.transform.position;
        Vector2 spawnPosition;
        int attempts = 0;
        do {
            float x = Random.Range(-8.0f, 8.0f);
            float y = Random.Range(-6f, 6f);
            spawnPosition = new Vector2(x, y);
            attempts++;
            if (attempts > 50) break;
        } while (Vector2.Distance(spawnPosition, playerPos) < minDistance);

        Instantiate(hpPrefab, spawnPosition, Quaternion.identity);
    }
}