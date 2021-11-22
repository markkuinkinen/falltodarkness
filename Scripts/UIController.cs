using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Text levelFinishedText;
    public Text gameOverText;
    public Text timeOutText;
    public Text Tutorial1;
    public Text Tutorial2;
    public Text Tutorial3;
    public Text youWin;

    public Image HP1;
    public Image HP2;
    public Image HP3;
    public Image HP4;
    
    private Player player;
    public float timer;   

    //timer variable
    public Text countdownDisplay;
    public int timeLimit;
    //public Image levelFinished;
    public bool forcedContinue;
    public bool showHP;

    //score variables
    public Text scoreText;
    public static int score = 0; //Access from other scripts by writing UIController.score

    public static int playerHP = 3;
    public int updateHP;

    void Start()
    {
        levelFinishedText.enabled = false;
        gameOverText.enabled = false;
        forcedContinue = false;
        timeLimit = 120;
        youWin.enabled = false;
        timeOutText.enabled = false;

        Tutorial1.enabled = false;
        Tutorial2.enabled = false;
        Tutorial3.enabled = false;

        updateHP = playerHP;

        player = FindObjectOfType<Player>(); //this finds a player type script from the scene

        StartCoroutine(CountdownToLoss());
    }

    IEnumerator CountdownToLoss()
    {
        while (timeLimit >= 0 && !player.isDead)
        {
            countdownDisplay.text = timeLimit.ToString();

            yield return new WaitForSeconds(1f);

            timeLimit--;
        }
        player.isDead = true;

    }


    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score.ToString();

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level_8"))
        {
            timer += Time.deltaTime;
            if (timer > 4)
            {
                player.isDead = true;
                youWin.enabled = true;
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level_3")) 
        {
            showHP = true;
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level_4") && player.checkPointReached && player.isDead && Input.GetKeyDown(KeyCode.Space)) //&& timeLimit > 0)
        {
            gameOverText.enabled = false;
            timeOutText.enabled = false;
            player.anim.SetBool("Death", false);
            player.isDead = false;
            player.canControl = true;

            player.anim.SetBool("Respawn", true);
            player.transform.position = new Vector3(-22.12f, 22.11f, 0);
            score = (score / 2);
            playerHP = 3;
            Invoke("resetRespawn", 0.5f);
            timeLimit = 120;
            StartCoroutine(CountdownToLoss());
        }

        if (player.tut1)
        {
            Tutorial1.enabled = true;
        } else
        {
            Tutorial1.enabled = false;
        }

        if (player.tut2)
        {
            Tutorial2.enabled = true;
        } else
        {
            Tutorial2.enabled = false;
        }

        if (player.tut3)
        {
            Tutorial3.enabled = true;
        } else
        {
            Tutorial3.enabled = false;
        }

        if (showHP)
        {
            healthSystem();
            player.skeletonMode = true;
        }

        if (player.levelFinished)
        {
            levelFinishedText.enabled = true;
        }

        if (timeLimit <= 0)
        {
            timeOutText.enabled = true;
        }

        else
        {
            timeOutText.enabled = false;
        }

        if (player.isDead && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Level_8") && timeLimit > 0)
        {
            gameOverText.enabled = true;
        }

        if ((player.isDead) && Input.GetKeyDown(KeyCode.Space) && !player.checkPointReached && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Level_8"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            score = (score/2);
            playerHP = 3;
        }

        if ((player.isDead) && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
            score = 0;
        }

        if (player.levelFinished && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (player.forcedContinue)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    void healthSystem()
    {

        if (playerHP == 4)
        {
            HP1.enabled = true;
            HP2.enabled = true;
            HP3.enabled = true;
            HP4.enabled = true;
        }
        else if (playerHP == 3)
        {
            HP1.enabled = true;
            HP2.enabled = true;
            HP3.enabled = true;
            HP4.enabled = false;
        }
        else if (playerHP == 2)
        {
            HP1.enabled = true;
            HP2.enabled = true;
            HP3.enabled = false;
            HP4.enabled = false;
        } 
        else if (playerHP == 1)
        {
            HP1.enabled = true;
            HP2.enabled = false;
            HP3.enabled = false;
            HP4.enabled = false;

        } else
        {
            HP1.enabled = false;
            HP2.enabled = false;
            HP3.enabled = false;
            HP4.enabled = false;
            player.isDead = true;
        }
    }

    void resetRespawn()
    {
        player.anim.SetBool("Respawn", false);
        timeLimit = 120;
    }
}
