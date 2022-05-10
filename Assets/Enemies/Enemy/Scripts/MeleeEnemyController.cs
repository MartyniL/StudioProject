﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeEnemyController : MonoBehaviour
{
    public GameObject weaponRotation, Player, Model;
    public Slider healthBar;
    public ParticleSystem smoke;
    public float Range, Vision, maxAttackCooldown, minAttackCooldown, maxHealth, damage;
    float health;
    public LayerMask playerLayer;
    bool canAttack = true;
    bool dead;
    bool inPlayerHitbox;
    AudioSource gunshot, reload;
    public Rigidbody body;
    public Animator anim;

    private void OnEnable()
    {
        dead = false;
        AudioSource[] source = GetComponents<AudioSource>();
        //weaponObject = weapon.spawnPrefab(weaponRotation.transform);
        gunshot = source[0];
        reload = source[1];
        //StartCoroutine(weapon.cooldown(reload));
        EventManager.OnAttack += TakeMeleeDamage;
        health = maxHealth;
        inPlayerHitbox = false;
        healthBar.maxValue = maxHealth;
        healthBar.minValue = 0;
        healthBar.value = health;
        body = GetComponent<Rigidbody>();
    }

    void TakeMeleeDamage(float damage)
    {
        if (inPlayerHitbox)
        {
            health -= damage;
        }
    }

    IEnumerator die()
    {
        EventManager.TriggerKill();
        //weaponObject.SetActive(false);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        smoke.Play();
        yield return new WaitForSeconds(3.0f);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ShotgunPellet":
                health -= 5.0f;
                break;
            case "Bullet":
                health -= 15.0f;
                break;
            case "Arrow":
                health -= 10.0f;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHitbox")
        {
            inPlayerHitbox = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerHitbox")
        {
            inPlayerHitbox = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (IsPlayerInSight() && !dead)
        {
            MoveTowardsPlayer();
            AimAtPlayer();
        }

        if (health < maxHealth)
        {
            health += 10.0f * Time.deltaTime;
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthBar.value = health;

        if (health <= 0 && !dead)
        {
            dead = true;
            StartCoroutine(die());
        }
    }

    void MoveTowardsPlayer()
    {
        float DistanceOnX = Player.transform.position.x - transform.position.x;

        if(DistanceOnX < -2)
        {
            Model.transform.eulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
            body.AddForce(new Vector3(-0.5f, 0.0f, 0.0f), ForceMode.Impulse);
        }
        else if (DistanceOnX > 2)
        {
            Model.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
            body.AddForce(new Vector3(0.5f, -0.0f, 0.0f), ForceMode.Impulse);
        }
    }

    void AimAtPlayer()
    {
        weaponRotation.transform.LookAt(Player.transform.position);
        if (IsPlayerInRange() && canAttack)
        {
            anim.SetTrigger("IsAttack");
            EventManager.TriggerEnemyAttack(damage);
            canAttack = false;
            StartCoroutine(cooldown());
        }
    }

    bool IsPlayerInSight()
    {
        Vector3 playerPos = Player.transform.position;
        Vector3 direction = playerPos - transform.position;
        if (Physics.Raycast(transform.position, direction, Vision, playerLayer))
        {
            return true;
        }
        return false;
    }

    bool IsPlayerInRange()
    {
        Vector3 playerPos = Player.transform.position;
        Vector3 direction = playerPos - transform.position;
        if (Physics.Raycast(transform.position, direction, Range, playerLayer))
        {
            return true;
        }
        return false;
    }

    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(Random.Range(minAttackCooldown, maxAttackCooldown));
        canAttack = true;
    }
}
