using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject gameStateController;
    GameStateController myGameStateController;

    public void PlayGame() {
//        gameObject.SetActive(false);
        myGameStateController.CurrentGameState = GameStateController.GameState.STATE_TRANSITION_TO_PLAYING;
        myGameStateController.fCountdown = 5;
    }

    // Start is called before the first frame update
    void Start()
    {
        myGameStateController = gameStateController.GetComponent<GameStateController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
