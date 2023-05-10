using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private PossibleScenes sceneToLoad;
    private void OnTriggerEnter(Collider other) 
    {
        SceneManager.LoadScene(sceneToLoad.ToString());
    }
}

// Must match exact scene names!
public enum PossibleScenes
{
    HomeHub,
    PigeonDemo
}
