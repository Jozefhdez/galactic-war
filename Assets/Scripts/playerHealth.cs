using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float health;
    public float maxHealth;
    public Image healthBar;
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateHealthBar()
    {
        healthBar.fillAmount = math.clamp(health / maxHealth, 0, 1);
        if (healthBar.fillAmount > 0.7f)
            healthBar.color = Color.yellow;
        else if (healthBar.fillAmount >= 0.25f)
            healthBar.color = Color.red;
        else
            healthBar.color = Color.red;
        }

}
