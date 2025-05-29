using UnityEngine;
using System.Collections;
using System.Linq;

public class shipScript : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public float velocity = 5;
    public float boostSpeed = 15f; // Velocidad en boost
    public float boostStaminaCost = 40f; // Gasto de estamina por segundo en boost
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

    // Stamina variables
    public float maxStamina = 100f;
    public float stamina = 100f;
    public float staminaRegenRate = 10f;
    public UnityEngine.UI.Image staminaBar;

    // Shield variables
    private int shieldHits = 0;
    public GameObject shieldVisual;

    // Engine effect
    public GameObject engineEffect;

    public UnityEngine.UI.Image marcoHealth;
    public UnityEngine.UI.Image marcoStamina;

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Ship");
        bulletPrefab.layer = LayerMask.NameToLayer("Bullet");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ship"), LayerMask.NameToLayer("Bullet"));

        if (engineEffect == null)
            engineEffect = GetComponentInChildren<Transform>(true)
                .GetComponentsInChildren<Transform>(true)
                .FirstOrDefault(t => t.CompareTag("Engine"))?.gameObject;

        if (engineEffect != null)
            engineEffect.SetActive(false);
    }

    void Update()
    {
        // Movimiento y boost
        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveDir += Vector3.up;
        if (Input.GetKey(KeyCode.A)) moveDir += Vector3.left;
        if (Input.GetKey(KeyCode.D)) moveDir += Vector3.right;
        if (Input.GetKey(KeyCode.S)) moveDir += Vector3.down;
        moveDir.Normalize();

        bool boosting = Input.GetKey(KeyCode.Space) && stamina > 0.1f;

        if (boosting)
        {
            if (engineEffect != null && !engineEffect.activeSelf)
                engineEffect.SetActive(true);

            transform.position += moveDir * boostSpeed * Time.deltaTime;
            stamina -= boostStaminaCost * Time.deltaTime;
            stamina = Mathf.Max(stamina, 0f);

            if (stamina <= 0f && engineEffect != null)
                engineEffect.SetActive(false);
        }
        else
        {
            if (engineEffect != null && engineEffect.activeSelf)
                engineEffect.SetActive(false);

            transform.position += moveDir * velocity * Time.deltaTime;

            // Regenerar estamina
            if (stamina < maxStamina)
            {
                stamina += staminaRegenRate * Time.deltaTime;
                stamina = Mathf.Min(stamina, maxStamina);
            }
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction.normalized;

        // Disparo
        if (Input.GetMouseButton(0) && fireTimer < 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }

        // Actualizar barra de estamina
        if (staminaBar != null)
            staminaBar.fillAmount = stamina / maxStamina;

        // Check if the ship goes out of bounds
        Vector3 position = transform.position;
        if (position.x > 9.5 || position.x < -9.5 || position.y > 5 || position.y < -5)
        {
            playerHealthBar.health -= damageBounds;
            playerHealthBar.UpdateHealthBar();
            if (playerHealthBar.health <= 0)
            {
                Destroy(staminaBar.gameObject);
                Destroy(marcoHealth.gameObject);
                Destroy(marcoStamina.gameObject);
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
                Destroy(marcoHealth.gameObject);
                Destroy(marcoStamina.gameObject);
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

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        bullet.GetComponent<bulletScript>().SetSpeed(10f);
    }
}