using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth;
    float health;
    public AudioSource healthSFX;
    bool InHitbox;
    int NoHitboxes;
    private void Awake()
    {
        health = maxHealth;
        Debug.Log(health);
    }
    private void OnEnable()
    {
        
        EventManager.OnAIAttack += isHit;
        EventManager.OnDeath += die;
    }

    private void Update()
    {
        if (health <= 0)
        {
            EventManager.TriggerDeath();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        switch(other.gameObject.tag)
        {
            case "BananaHealthSmall":
                healthSFX.Play();
                Destroy(other.gameObject);
                health += maxHealth * 0.25f;
                if (health > maxHealth)
                {
                    health = maxHealth;
                }

                healthBar.fillAmount = (health / maxHealth);
                break;

            case "BananaHealthBig":
                Destroy(other.gameObject);
                health += maxHealth * 0.4f;
                if (health > maxHealth)
                {
                    health = maxHealth;
                }

                healthBar.fillAmount = (health / maxHealth);
                break;

            case "DeathPlane":
                EventManager.TriggerDeath();
                break;
            case "Hitbox":
                InHitbox = true;
                NoHitboxes++;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "Hitbox":
                NoHitboxes--;
                if(NoHitboxes == 0)
                {
                    InHitbox = false;
                }
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Bullet":
                health -= 10;
                healthBar.fillAmount = (health / maxHealth);
                break;
            case "ShotgunPellet":
                health -= 5;
                healthBar.fillAmount = (health / maxHealth);
                break;
        }
    }

    void die()
    {
        health = 0;
    }

    void isHit(float damage)
    {
        if(InHitbox)
        {
            Debug.Log(health);
            health -= damage;
            Debug.Log(health);
            healthBar.fillAmount = (health / maxHealth);
            Debug.Log((health / maxHealth));
        }
    }
}
