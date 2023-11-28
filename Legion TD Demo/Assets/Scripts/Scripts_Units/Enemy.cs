using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public int startHealth = 100;
    public float health;

    public int value = 10;
    public GameObject deathEffect;

    public float startSpeed;
    [HideInInspector]
    public float speed;

    [Header("Unity Stuff")]
    public Image healthBar;

    private bool isDead = false;

    void Start()
    {
        health = startHealth;
        startSpeed = speed;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Slow(float amount)
    {
        speed = startSpeed * (1f - amount);
    }

    void Die()
    {
        isDead = true;

        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
        PlayerStats.Money += value;
        Destroy(gameObject);
    }
}

