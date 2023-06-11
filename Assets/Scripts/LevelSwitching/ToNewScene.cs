using UnityEngine;

public class ToNewScene : MonoBehaviour
{
    
    [SerializeField] private PossibleScenes sceneToLoad;
    private void Update() {
        if(Input.GetKeyDown(KeyCode.N)) {
            SceneSwitcher.Instance.TransitionToScene(sceneToLoad);
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        SceneSwitcher.Instance.TransitionToScene(sceneToLoad);
    }
}

// Must match exact scene names!
public enum PossibleScenes
{
    TitleScreen,
    MagritteHouse,
    PigeonDemo,
    __AppleSearchLevelDeco_BACKUP,
    AppleSearchLevelDeco,
    RedLightGreenLight,
    DodgeTheDarkness   
}