using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GlobalSceneManager : MonoBehaviour
{
    // References to all virtual cams, to enable them when needed
    [SerializeField] private CinemachineVirtualCamera levelTransitionFollowPlayerVcam;
    [SerializeField] private CinemachineVirtualCamera level0StaticVcam;
    [SerializeField] private CinemachineVirtualCamera level1StaticVcam;
    [SerializeField] private CinemachineVirtualCamera level2StaticVcam;
    [SerializeField] private CinemachineVirtualCamera level3StaticVcam;
    [SerializeField] private CinemachineVirtualCamera level4FollowPlayerVcam;
    private CinemachineVirtualCamera _activeCam = null;
    // Stores for each scene if it has been loaded
    private Dictionary<PossibleScenes, bool> _levelStatusses = new Dictionary<PossibleScenes, bool>();
    // Start is called before the first frame update
    void Awake()
    {
        _levelStatusses.Add(PossibleScenes.MagritteHouse, true);
        _levelStatusses.Add(PossibleScenes.PigeonDemo, false);
    }

    public void LoadScene(PossibleScenes scene) 
    {
        if(!IsSceneLoaded(scene))
        {
            SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive);
            _levelStatusses[scene] = true;
            SetOneCamActive(levelTransitionFollowPlayerVcam);
        }
    }

    public void UnloadScene(PossibleScenes scene) 
    {
        if(IsSceneLoaded(scene))
        {
            SceneManager.UnloadSceneAsync(scene.ToString());
            Debug.Log(scene);
            // When unloading magritte house, switch cam to level 1
            if(scene == PossibleScenes.MagritteHouse)
            {
                Debug.Log("Setting active camera to level 1 static");
                SetOneCamActive(level1StaticVcam);
            }
            _levelStatusses[scene] = false;
        }
    }

    private bool IsSceneLoaded(PossibleScenes scene)
    {
        Debug.Log(_levelStatusses[scene]);
        return _levelStatusses[scene];
    }
    private void SetOneCamActive(CinemachineVirtualCamera cam)
    {
        _activeCam = cam;
        levelTransitionFollowPlayerVcam.Priority = cam.name.Equals(levelTransitionFollowPlayerVcam.name) ? 10 : 0;
        level0StaticVcam.Priority = cam.name.Equals(level0StaticVcam.name) ? 10 : 0;
        level1StaticVcam.Priority = cam.name.Equals(level1StaticVcam.name) ? 10 : 0;
        //level2StaticVcam.Priority = cam.name.Equals(level2StaticVcam.name) ? 10 : 0;
        //level3StaticVcam.Priority = cam.name.Equals(level3StaticVcam.name) ? 10 : 0;
        level4FollowPlayerVcam.Priority = cam.name.Equals(level4FollowPlayerVcam) ? 10 : 0;
    }
}
