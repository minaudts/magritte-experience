using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintPrompt : MonoBehaviour
{
    public static bool menuOpened;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (menuOpened)
        {
            gameObject.SetActive(false);
        }
    }
}
