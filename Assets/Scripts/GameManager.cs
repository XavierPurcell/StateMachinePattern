using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
          float randomNumber = Random.Range(0, 2);

        if (randomNumber == 1)
        {
            Application.targetFrameRate = 30;
        }
        else
        {
            Application.targetFrameRate = 300;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && paused == false)
        {
            paused = true;
            PauseGame();

        }
        else if (Input.GetKeyDown(KeyCode.P) && paused == true)
        {
            paused = false;
            ResumeGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }

}
