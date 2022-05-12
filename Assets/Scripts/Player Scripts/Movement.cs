using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    Camera cam;
    Rigidbody body;
    public Transform modelTrans;
    public Animation_Con animController;

    public AudioSource coinSFX, healthSFX, bgMusic;

    public TextMeshProUGUI CoinCounter;
    public Vector3 jumpForce;

    public float moveSpeed, maxJumpTime, InitialJump, maxHealth, airSpeedMultiplier, maxSpeed;
    float health, jumpTime;
    bool canJump, jumping, canLand;
    public bool falling;
    public static int coins;
    public LayerMask player;
    Grapple hook;

    void Awake()
    {
        hook = GetComponent<Grapple>();
        AudioListener.volume = PlayerPrefs.GetFloat("master");
        coinSFX.volume = PlayerPrefs.GetFloat("sfx");
        healthSFX.volume = PlayerPrefs.GetFloat("sfx");
        bgMusic.volume = PlayerPrefs.GetFloat("music");

        cam = Camera.main;
        canJump = true;
        falling = false;
        body = GetComponent<Rigidbody>();

        coins = 0;
        CoinCounter.SetText(coins.ToString());
    }

    private void Update()
    {
        Move();

        Jump();
    }

    void Jump()
    {
        if(!canLand)
        {
            if(!grounded())
            {
                canLand = true;
            }
        }

        if(canJump && grounded() && Input.GetButtonDown("Jump"))
        {
            jumpTime = 0f;
            canJump = false;
            jumping = true;
            falling = true;
            canLand = false;
            body.AddForce(jumpForce, ForceMode.Impulse);
        }

        else if(jumping)
        {
            if(jumpTime < maxJumpTime)
            {
                falling = true;
                canLand = false;
                jumpTime += Time.deltaTime;
                body.AddForce(new Vector3(0.0f, 9.81f, 0.0f) * Time.deltaTime, ForceMode.VelocityChange);
            }
            else
            {
                jumping = false;
            }
            if(!Input.GetButton("Jump"))
            {
                jumping = false;
                if(grounded() && canLand)
                {
                    animController.land();
                    falling = false;
                    canJump = true;
                    canLand = false;
                }
            }
        }

        else if(grounded() & canLand)
        {
            animController.land();
            falling = false;
            canJump = true;
            canLand = false;
        }

    }

    public bool grounded()
    {
        Vector3 checkPos = transform.position - new Vector3(0.0f, transform.localScale.y/2 - 0.5f, 0.0f);
        return Physics.CheckSphere(checkPos, 0.5f, ~player);
    }

    void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);

        if (!hook.isGrappling && grounded())
        {
            body.AddForce(movement * moveSpeed * Time.deltaTime, ForceMode.Impulse);
            body.velocity = (movement * moveSpeed) + new Vector3(0.0f, body.velocity.y, 0.0f);
        }
        else
        {
            if (Mathf.Abs(body.velocity.x) < maxSpeed)
            {
                body.AddForce(movement * moveSpeed * airSpeedMultiplier * Time.deltaTime, ForceMode.Impulse);
            }
            //body.velocity = (movement * moveSpeed * airSpeedMultiplier) + new Vector3(0.0f, body.velocity.y, 0.0f);
        }

        if(Input.GetAxis("Horizontal") < 0)
        {
            modelTrans.eulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
        }
        else
        {
            modelTrans.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "DeathPlane":
                EventManager.TriggerDeath();
                break;
            case "Win":
                EventManager.TriggerWin();
                break;
            case "Coin":
                Destroy(other.gameObject);
                coinSFX.Play();
                coins++;
                CoinCounter.SetText(coins.ToString());
                break;

        }
    }
}

