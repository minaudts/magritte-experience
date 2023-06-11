using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : SingletonPersistent<MenuCanvas>
{
    [SerializeField] private GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
    public void RestartGame()
    {
        //TODO?
        Debug.Log("Restart game");
    }

    public void ShowMenu()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
    }
    public void HideMenu()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }
}
