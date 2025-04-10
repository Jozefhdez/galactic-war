using UnityEngine;

public class enemiesSpawnerScript : MonoBehaviour
{
    public GameObject enemies;
    private float spawnRate = 1;
    private float timer = 2;
    public shipScript isAlive;
    public bool isAliveVar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        isAliveVar = isAlive.isAliveFunction();
        if (isAliveVar == false)
        {
            return;
        }
        if (timer < spawnRate)
        {
            timer += Time.deltaTime; // Increment timer based on the time elapsed since the last frame
        }
        else
        {
            spawnEnemy();
            timer = 0; // Reset the timer to 0 to start counting again for the next spawn
        }
    }

        void spawnEnemy(){
            float x = Random.Range(-8.0f, 8.0f); // Adjust the range based on your playable zone
            float y = Random.Range(-6f, 6f);
            Vector2 spawnPosition = new Vector2(x, y);
            Instantiate(enemies, spawnPosition, Quaternion.identity);
        }
}
