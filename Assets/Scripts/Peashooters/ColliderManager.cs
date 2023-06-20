using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    public GameObject[] lightColliders;
    int inNColliders;
    bool isInLight;

    public GameObject magritte;
    float timeInDarkness;
    public float maxTimeInDarkness;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckColliders();
        if (!isInLight)
        {            
            OutOfLight();
        }
        else
        {
            timeInDarkness = 0;
        }
    }

    void CheckColliders()
    {
        for (int i = 0; i < lightColliders.Length; i++)
        {
            if (lightColliders[i].GetComponent<LightCollider>().isInLight)
            {
                inNColliders += 1;
            }
        }

        if (inNColliders > 0)
        {
            isInLight = true;
        }
        else
        {
            isInLight = false;
        }
        //Debug.Log(isInLight + ", player is in " + inNColliders + " colliders");
        inNColliders = 0;
    }

    void OutOfLight()
    {
        timeInDarkness += Time.deltaTime;
        Debug.Log("time in darkness: " + timeInDarkness);
        if(timeInDarkness > maxTimeInDarkness)
        {
            magritte.GetComponent<Magritte>().Respawn();
        }
    }
}
