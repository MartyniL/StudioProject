using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{


    public GameObject pauseMenu, Death, winning;
    public TextMeshProUGUI CoinCounter, EnemyKills;
    static public bool paused = false;
    bool canPause;
    int kills;


    private void OnEnable()
    {
        EventManager.OnDeath += died;
        EventManager.OnWin += Win;
        EventManager.Enemykill += OnKill;
    }

    private void OnDisable()
    {
        EventManager.OnDeath -= died;
        EventManager.OnWin -= Win;
        EventManager.Enemykill -= OnKill;

    }

    void OnKill()
    {
        kills++;
    }

    void Win()
    {
        Time.timeScale = 0;
        CoinCounter.SetText(PlayerMove.coins.ToString());
        EnemyKills.SetText(kills.ToString());
        winning.SetActive(true);
        paused = true;
        canPause = false;
    }
    void died()
    {
        Time.timeScale = 0;
        Death.SetActive(true);
        paused = true;
        canPause = false;
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
        paused = false;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        //Time.timeScale = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        canPause = true;
        kills = 0;
        Time.timeScale = 1;
        paused = false;
        winning.SetActive(false);
        pauseMenu.SetActive(false);
        Death.SetActive(false);
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        paused = !paused;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        paused = !paused;
    }

    // Update is called once per frame
    void Update()
    {
        if (canPause)
        {
            if (Input.GetKeyDown("escape"))
            {
                if (paused == false)
                {
                    Pause();
                }
                else if (paused == true)
                {
                    Unpause();
                }
            }
        }
    }
}
