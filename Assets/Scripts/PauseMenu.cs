using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    
    public GameObject pauseMenu;
    static public bool paused = false;

    public void GoToMenu()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
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
