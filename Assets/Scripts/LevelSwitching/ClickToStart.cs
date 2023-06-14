using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToStart : MonoBehaviour
{
    public void StartExperience()
    {
        SceneSwitcher.Instance.TransitionToScene(PossibleScenes.Level0MagritteHouse);
    }
}
