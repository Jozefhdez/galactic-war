using System;
using UnityEngine;

public class enemiesScript : MonoBehaviour
{
    private Rigidbody2D myRigidBodyShip;
    private float velocityEnemy = 4;
    public AudioSource explosionSound;
    public LogicScript logic;

    // Gun variables
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [Range(0.1f, 2f)]
    [SerializeField] private float fireRate = 1f;
    private float fireTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        logic = GameObject.FindWithTag("Logic").GetComponent<LogicScript>();
        GameObject ship = GameObject.FindWithTag("ship");
        if (ship != null)
        {
            myRigidBodyShip = ship.GetComponent<Rigidbody2D>();
        }
        else
        {
            myRigidBodyShip = null;
        }
        // Ignore collision between ship and its bullets
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("BulletEnemy"));
        fireTimer = UnityEngine.Random.Range(0f, fireRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (myRigidBodyShip == null)
        {
            return;
        }
        Vector3 shipPosition = myRigidBodyShip.position;

        Vector2 direction = new Vector2(shipPosition.x - transform.position.x, shipPosition.y - transform.position.y);

        transform.up = direction;

        transform.position = Vector2.MoveTowards(transform.position, shipPosition, velocityEnemy * Time.deltaTime);

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            logic.addScore(1);
        }
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("ship"))
        {
            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound.clip, transform.position);
            }

            Destroy(gameObject);
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        bullet.layer = LayerMask.NameToLayer("BulletEnemy");
        Collider2D bulletCol = bullet.GetComponent<Collider2D>();
        Collider2D enemyCol = GetComponent<Collider2D>();
        if (bulletCol != null && enemyCol != null)
        {
            Physics2D.IgnoreCollision(bulletCol, enemyCol);
        }
        bullet.GetComponent<bulletScript>().SetSpeed(8f);
    }
}
