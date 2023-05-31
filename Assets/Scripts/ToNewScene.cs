using UnityEngine;

public class ToNewScene : MonoBehaviour
{
    //private GlobalSceneManager _sceneManager;
    private void Start() {
        //_sceneManager = GameObject.FindObjectOfType<GlobalSceneManager>();
    }
    [SerializeField] private PossibleScenes sceneToLoad;
    private void OnTriggerEnter(Collider other) 
    {
        //SceneManager.LoadSceneAsync(sceneToLoad.ToString(), LoadSceneMode.Single);
        //_sceneManager.LoadScene(sceneToLoad);
        SceneSwitcher.Instance.TransitionToScene(sceneToLoad, null);
    }
}

// Must match exact scene names!
public enum PossibleScenes
{
    MagritteHouse,
    PigeonDemo,
    __AppleSearchLevelDeco_BACKUP,
    RedLightGreenLight,
    DodgeTheDarkness   
}