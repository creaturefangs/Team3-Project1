using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public GameObject healthUI;
    private int damageAmount = 25; // player takes 25 damage each time

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDamage();
    }

    public void TakeDamage(int amount)
    {
        //player takes damage
        currentHealth -= amount;
        if (currentHealth <= 0) { Die(); }
    }

    void UpdateDamage()
    {
        Color currentAlpha = healthUI.GetComponent<RawImage>().color;
        currentAlpha.a = 1 - (currentHealth / maxHealth);
        healthUI.GetComponent<RawImage>().color = currentAlpha;
    }

    public void RestoreHealth(int amount)
    {
        currentHealth += amount;
        // Ensure the current health doesn't exceed the maximum health.
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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
            //TakeDamage(damageAmount);
            //Debug.Log("Player has been attacked.");
        }
    }

}
