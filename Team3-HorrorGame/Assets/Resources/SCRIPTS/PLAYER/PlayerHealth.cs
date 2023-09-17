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

    private GameObject overlay;
    private DevTools devtools;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        overlay = GameObject.Find("BlinkOverlay");
        devtools = GameObject.Find("PlayerController").GetComponent<DevTools>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDamage();
    }

    public void TakeDamage(int amount)
    {
        if (!devtools.godMode)
        {
            currentHealth -= amount;
            if (currentHealth <= 0) { Die(); }
        }
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
        overlay.GetComponent<Animator>().Play("HealOverlay");
    }

    public void Die()
    {
        //load game over screen
        SceneManager.LoadScene("GAMEOVER");
    }

    // Example usage
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.gameObject.name == "BearTrap")
            {
                TakeDamage(25);
                other.gameObject.GetComponent<Animator>().Play("beartrapclose");
                GameObject.Find("beartrapSFX").GetComponent<AudioSource>().Play();
                other.gameObject.tag = "Untagged";
            }
        }
    }

}
