using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    //Private variables
    private Camera cam;
    private Rigidbody playerRB;
    public Transform playerTrans, modelTrans;

    //Public Objects
    public AudioSource coinSFX, healthSFX, bgMusic;
    public Transform playerCenter;
    public Vector3 cameraOffset;
    //public Image healthBar;

    public TextMeshProUGUI CoinCounter;

    //Public Variables
    public float moveSpeed, jumpForce, timeInAir, maxJumpTime, initialJump, maxHealth;
    float health;
    public bool canJump = true, alive = true;
    public static int coins;


    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("master");
        coinSFX.volume = PlayerPrefs.GetFloat("sfx");
        healthSFX.volume = PlayerPrefs.GetFloat("sfx");
        bgMusic.volume = PlayerPrefs.GetFloat("music");

        //Assigning some variables
        cam = Camera.main;
        canJump = true;
        playerRB = GetComponent<Rigidbody>();
        playerTrans = GetComponent<Transform>();
        coins = 0;
        CoinCounter.SetText(coins.ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "DeathPlane")
        {
            EventManager.TriggerDeath();
        }
        if(other.gameObject.tag == "Win")
        {
            EventManager.TriggerWin();
        }
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            coinSFX.Play();
            coins++;
            CoinCounter.SetText(coins.ToString());
        }
    }

    void Update()
    {

        PlayerRotation();

        //Camera offset
        //cam.transform.position = playerCenter.position + cameraOffset;

        RaycastHit hit;
        //SphereCast below the player to check for ground
        Debug.DrawRay(playerCenter.position, Vector3.down * 0.15f, Color.blue);
        Debug.DrawRay(playerCenter.position + (Vector3.down * 0.15f), Vector3.down * (playerTrans.localScale.x / 2), Color.red);
        if (Physics.SphereCast(playerCenter.position, playerTrans.localScale.x / 2, Vector3.down, out hit, 0.15f) && Input.GetAxis("Jump") == 0)
        {
            Debug.Log("I am hitting");
            if (hit.collider.tag != "Pickups")
            {
                canJump = true;
                timeInAir = 0f;
                playerRB.velocity = new Vector3(0, playerRB.velocity.y, 0);
            }
        }

        //Jump Script
        if (Input.GetButton("Jump") && canJump == true && GetComponent<Grapple>().isGrappling == false)
        {
            /*if (timeInAir < maxJumpTime)
            {
                //At the beginning of the jump, set the velocity to make the jump more realistic
                if (timeInAir == 0)
                {
                    playerRB.AddForce(Vector3.up * initialJump, ForceMode.VelocityChange);
                }
                playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                timeInAir += Time.deltaTime;
            }*/
            //If the player lets go of space the jump stops
            if (timeInAir < maxJumpTime)
            {
                if (timeInAir == 0)
                {
                    playerRB.AddForce(Vector3.up * initialJump, ForceMode.Force);
                }
                playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Force);
                timeInAir += Time.deltaTime;
            }
            else
            {
                canJump = false;
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            canJump = false;
        }
    }

    void PlayerRotation()
    {
        float velX = playerRB.velocity.x;
        if(velX < 0)
        {
            modelTrans.eulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
        }
        else if (velX > 0)
        {
            modelTrans.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
        }
    }

    private void LateUpdate()
    {
        
    }

    //Other input in fixed update
    void FixedUpdate()
    {
        if (alive == true && GetComponent<Grapple>().isGrappling == false)
        {
            YourInput();
        }
    }

    //User input
    void YourInput()
    {
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            playerRB.velocity = new Vector3(-moveSpeed, playerRB.velocity.y, 0);
        }

        else if (Input.GetAxisRaw("Horizontal") == 1)
        {
            playerRB.velocity = new Vector3(moveSpeed, playerRB.velocity.y, 0);
        }

        else if (GetComponent<Grapple>().isGrappling == false)
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x / 1.07f, playerRB.velocity.y, 0);
        }
    }
}