using UnityEngine;
using System.Collections;

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

    // Dash variables
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float dashStaminaCost = 100f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashSpeed = 20f;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    // Stamina variables
    public float maxStamina = 100f;
    public float stamina = 100f;
    public float staminaRegenRate = 10f; // Stamian regeneration per second
    public UnityEngine.UI.Image staminaBar;

    // Shield variables
    private int shieldHits = 0;
    public GameObject shieldVisual;

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

        // Logica para el dash
        if (!isDashing)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0f && stamina >= dashStaminaCost)
            {
                Dash();
                stamina -= dashStaminaCost;
                dashCooldownTimer = dashCooldown;
            }

            // Regenerar estamina
            if (stamina < maxStamina)
            {
                stamina += staminaRegenRate * Time.deltaTime;
                stamina = Mathf.Min(stamina, maxStamina);
            }

            // Actualizar staminaBar
            if (staminaBar != null)
                staminaBar.fillAmount = stamina / maxStamina;
        }

        // Check if the ship goes out of bounds
        Vector3 position = transform.position;
        if (position.x > 9.5 || position.x < -9.5 || position.y > 5 || position.y < -5)
        {
            playerHealthBar.health -= damageBounds;
            playerHealthBar.UpdateHealthBar();
            if (playerHealthBar.health <= 0)
            {
                Destroy(staminaBar.gameObject);
                Destroy(gameObject);
                logic.gameOver();
                shipIsFine = false;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (shieldHits > 0)
        {
            shieldHits--;
            if (shieldHits == 0 && shieldVisual != null)
                shieldVisual.SetActive(false);
            Destroy(collision.gameObject);
            return;
        }

        playerHealthBar.health -= damage;
        playerHealthBar.UpdateHealthBar();
        Destroy(collision.gameObject);

        if (playerHealthBar.health <= 0)
        {
            if (staminaBar != null)
                Destroy(staminaBar.gameObject);
            Destroy(gameObject);
            logic.gameOver();
            shipIsFine = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.CompareTag("Shield"))
    {
        shieldHits = 3;
        shieldVisual.SetActive(true);
        Destroy(other.gameObject);
        return;
    }

    if (other.gameObject.CompareTag("HP"))
    {
        playerHealthBar.health = playerHealthBar.maxHealth;
        playerHealthBar.UpdateHealthBar();
        Destroy(other.gameObject);
        return;
    }
    
}

    public bool isAliveFunction()
    {
        return shipIsFine;
    }

    private void Dash()
    {
        if (!isDashing)
            StartCoroutine(DashRoutine());
    }
    private IEnumerator DashRoutine()
    {
        isDashing = true;
        float elapsed = 0f;
        Vector3 dashDirection = transform.up;

        while (elapsed < dashDuration)
        {
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        bullet.GetComponent<bulletScript>().SetSpeed(10f);
    }
}
