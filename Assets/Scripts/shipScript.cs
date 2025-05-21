using UnityEngine;

public class shipScript : MonoBehaviour
{

    public Rigidbody2D myRigidBody;
    public float velocity = 5;
    public bool shipIsFine = true;
    public LogicScript logic;
    public playerHealth playerHealthBar;
    public float damage = 25f;
    public float damageBounds = 100f;
    
    // Gun variables
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [Range(0.1f, 2f)]
    [SerializeField] private float fireRate = 0.5f;
    private float fireTimer;
    private void Start()
    {
        // Set the ship to a specific layer
        gameObject.layer = LayerMask.NameToLayer("Ship");

        // Set the bullet prefab to a specific layer
        bulletPrefab.layer = LayerMask.NameToLayer("Bullet");

        // Ignore collision between ship and its bullets
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ship"), LayerMask.NameToLayer("Bullet"));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W)){
            transform.position += Vector3.up * velocity * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.A)){
            transform.position += Vector3.left * velocity * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.D)){
            transform.position += Vector3.right * velocity * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.S)){
            transform.position += Vector3.down * velocity * Time.deltaTime;
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction.normalized;

        if(Input.GetMouseButton(0) && fireTimer < 0f){
            Shoot();
            fireTimer = fireRate;
        } else {
            fireTimer -= Time.deltaTime;
        }
        // Check if the ship goes out of bounds
        Vector3 position = transform.position;
        if (position.x > 9.5 || position.x < -9.5 || position.y > 5 || position.y < -5)
        {
            playerHealthBar.health -= damageBounds;
            playerHealthBar.UpdateHealthBar();
            if (playerHealthBar.health <= 0)
            {
                Destroy(gameObject);
                logic.gameOver();
                shipIsFine = false;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision){
            playerHealthBar.health -= damage;
            playerHealthBar.UpdateHealthBar();
            Destroy(collision.gameObject);

            if (playerHealthBar.health <= 0)
            {
                Destroy(gameObject);
                logic.gameOver();
                shipIsFine = false;
            }

    }

    public bool isAliveFunction(){
        return shipIsFine;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        bullet.GetComponent<bulletScript>().SetSpeed(10f);
    }
}
