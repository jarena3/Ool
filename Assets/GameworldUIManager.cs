using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class GameworldUIManager : MonoBehaviour
{

    public GameObject PauseMenu;
    public Blur CameraBlur;
    public GameManager Manager;


    void Start()
    {
        Manager = FindObjectOfType<GameManager>();
        CameraBlur.enabled = false;
    }

    public void Pause()
    {
        Manager.GameStopwatch.Stop();
        Time.timeScale = 0;
        CameraBlur.enabled = true;
        PauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        Manager.GameStopwatch.Start();
        Time.timeScale = 1;
        CameraBlur.enabled = false;
        PauseMenu.SetActive(false);
    }

}
