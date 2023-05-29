using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterNewScene : MonoBehaviour
{
    [SerializeField] private PossibleScenes sceneToUnload;
    [SerializeField] private GameObject airBridge;
    private GlobalSceneManager _sceneManager;
    private void Awake() {
        _sceneManager = GameObject.FindObjectOfType<GlobalSceneManager>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) {
        airBridge.SetActive(false);
        _sceneManager.UnloadScene(sceneToUnload);
    }
}
