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
        if(other.GetComponent<Magritte>())
        {
            SceneSwitcher.Instance.TransitionToScene(sceneToLoad);
        }
    }
}

// Must match exact scene names!
public enum PossibleScenes
{
    TitleScreen,
    Level0MagritteHouse,
    Level1Pigeons,
    Level2KeySearch,
    Level3RedLightGreenLight,
    OutroCutscene,
}