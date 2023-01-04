using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStateController : MonoBehaviour
{

    public GameObject myCamera;
    public GameObject myGhostController;
    public TextMeshProUGUI myCountdownObj;
    public ControlBrickScript myControlBrick;

    public GameObject myPauseScreen;

    public GameObject myMainMenu;

    public float fCountdown;

    public enum GameState {
        STATE_MENU,
        STATE_TRANSITION_TO_PLAYING,
        STATE_TRANSITION_TO_MENU,
        STATE_TRANSITION_TO_PAUSE,
        STATE_PAUSED,
        STATE_PLAYING
    };

    public GameState CurrentGameState;

    // Start is called before the first frame update
    void Start()
    {
        myGhostController.SetActive(false);
        myControlBrick.gameObject.SetActive(false);

        myPauseScreen.GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
        UpdateCountdown();

        if(CurrentGameState == GameState.STATE_PLAYING) {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                myMainMenu.SetActive(true);
                CurrentGameState = GameState.STATE_TRANSITION_TO_PAUSE;
                myPauseScreen.GetComponent<Canvas>().enabled = true;
            }
        }

        if(fCountdown == 0)
            myCountdownObj.text = "";
        else
            myCountdownObj.text = "Starting in ... " + ((int)fCountdown);
    }

    void UpdateCountdown()
    {
        if(fCountdown > 0) {
            fCountdown -= Time.deltaTime;
            if(fCountdown < 0) {
                fCountdown = 0;
            }
        }
    }

    void UpdateCamera()
    {
        if(CurrentGameState == GameState.STATE_TRANSITION_TO_PLAYING) {
            Vector3 start = myCamera.GetComponent<Camera>().transform.eulerAngles;
            myCamera.GetComponent<Camera>().transform.eulerAngles = Vector3.Lerp(start, new Vector3(0,180,0), 5 * Time.deltaTime);
            if(myCamera.GetComponent<Camera>().transform.eulerAngles.y >= 179
            && fCountdown == 0) {
                myCamera.GetComponent<Camera>().transform.eulerAngles = new Vector3(0,180,0);
                CurrentGameState = GameState.STATE_PLAYING;
                SetupGameStart();
            }
        }
        else if(CurrentGameState == GameState.STATE_TRANSITION_TO_MENU) {
            Vector3 start = myCamera.GetComponent<Camera>().transform.eulerAngles;
            myCamera.GetComponent<Camera>().transform.eulerAngles = Vector3.Lerp(start, new Vector3(0,0,0), 5 * Time.deltaTime);
            if(myCamera.GetComponent<Camera>().transform.eulerAngles.y <= 1) {
                CurrentGameState = GameState.STATE_MENU;
                myPauseScreen.GetComponent<Canvas>().enabled = false;
                CleanupGameEnd();
            }
        }
        else if(CurrentGameState == GameState.STATE_TRANSITION_TO_PAUSE) {
            Vector3 start = myCamera.GetComponent<Camera>().transform.eulerAngles;
            myCamera.GetComponent<Camera>().transform.eulerAngles = Vector3.Lerp(start, new Vector3(0,90,0), 5 * Time.deltaTime);
            if((int)myCamera.GetComponent<Camera>().transform.eulerAngles.y == 90) {
                CurrentGameState = GameState.STATE_PAUSED;
            }
        }
    }

    void SetupGameStart()
    {
        myMainMenu.SetActive(false);
        myGhostController.SetActive(true);
        myControlBrick.gameObject.SetActive(true);
    }

    void CleanupGameEnd()
    {
        myMainMenu.SetActive(true);
        myGhostController.SetActive(false);
        myControlBrick.gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        CurrentGameState = GameState.STATE_TRANSITION_TO_PLAYING;
        myPauseScreen.GetComponent<Canvas>().enabled = true;
    }

    public void ReturnToMenu()
    {
        CurrentGameState = GameState.STATE_TRANSITION_TO_MENU;
    }
}
