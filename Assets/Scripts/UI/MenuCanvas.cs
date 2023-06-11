using UnityEngine;


public class MenuCanvas : MonoBehaviour
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
        SceneSwitcher.Instance.TransitionToScene(PossibleScenes.TitleScreen);
    }

    public void ShowMenu()
    {
        menu.SetActive(true);
    }
    public void HideMenu()
    {
        menu.SetActive(false);
    }
}
