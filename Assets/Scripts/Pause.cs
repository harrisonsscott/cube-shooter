using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// place this script in the pause ui object

public class Pause : MonoBehaviour
{
    private Button PauseButton;
    public Button ResumeButton;
    public Button QuitButton;

    // the pause button's sprite is replaced with "resume" when it's pause, and "pause" and it's resumed
    public Texture resume; // triangle
    public Texture pause; // two bars

    public GameObject pauseMenu;
    private bool isPaused;

    private void Start() {
        isPaused = true;
        SwapSprite();
        PauseButton = gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();     
        PauseButton.onClick.AddListener(SwapSprite);   
        ResumeButton.onClick.AddListener(SwapSprite);
        QuitButton.onClick.AddListener(() => {
            GlobalVariables.isAlive = false;
            FindObjectOfType<Player>().Explode();
            SwapSprite();
        });
    }

    private void SwapSprite(){
        isPaused = !isPaused;
        gameObject.GetComponent<RawImage>().texture = isPaused ? resume : pause;
        pauseMenu.SetActive(isPaused);
        GlobalVariables.isPaused = isPaused;
    }
}
