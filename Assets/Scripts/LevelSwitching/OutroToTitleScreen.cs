using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroToTitleScreen : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
