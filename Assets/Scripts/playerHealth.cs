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
            healthBar.color = new Color32(0x2e, 0xcc, 0x71, 0xff); 
        else if (healthBar.fillAmount > 0.25f)
            healthBar.color = new Color32(0xf1, 0xc4, 0x0f, 0xff);
        else
            healthBar.color = new Color32(0xe7, 0x4c, 0x3c, 0xff);
    }

}
