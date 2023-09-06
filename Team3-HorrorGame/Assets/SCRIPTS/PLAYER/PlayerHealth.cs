using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Text healthUI; // Reference to a UI text element for displaying health.
    public GameObject pickupHealthkit;
    public GameObject pickupPills;
    private int damageAmount = 25; // player takes 25 damage each time

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageAmount)
    {
        //player takes damage
        currentHealth -= damageAmount;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        //update health ui, needs to be set active to different sprites, like the visibility icons, health 0/25/50/75/ = different sprites. for now when damaged, the one dmgUI is set active.
        // this code is placeholder
        if (healthUI != null)
        {
            healthUI.text = "Health: " + currentHealth.ToString();
        }
    }

    public void RestoreHealth(int amount)
    {
        currentHealth += amount;
        // Ensure the current health doesn't exceed the maximum health.
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
    }

    void Die()
    {
        //load game over screen
        SceneManager.LoadScene("GAMEOVER");
    }

    // Example usage
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Damage the player when colliding with an enemy.
            TakeDamage(damageAmount);
            Debug.Log("Player has been attacked.");
        }
    }

}
