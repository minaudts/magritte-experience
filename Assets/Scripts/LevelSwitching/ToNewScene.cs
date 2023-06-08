using UnityEngine;

public class ToNewScene : MonoBehaviour
{
    //private GlobalSceneManager _sceneManager;
    private void Start() {
        //_sceneManager = GameObject.FindObjectOfType<GlobalSceneManager>();
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.N)) {
            SceneSwitcher.Instance.TransitionToScene(sceneToLoad, null);
        }
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
    AppleSearchLevelDeco,
    RedLightGreenLight,
    DodgeTheDarkness   
}