using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    public GameObject[] lightColliders;
    public GameObject[] spotlights;
    int inNColliders;
    bool isInLight;
    public float lightRenderDistance;

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
        spotlightRender();
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

    void spotlightRender()
    {
        //URP render maar 8 lichten tegelijk, hiermee activeer je enkel de lichten dichtst bij magritte
        for (int i = 0; i < spotlights.Length; i++)
        {
            if(Vector3.Distance(magritte.gameObject.transform.position, spotlights[i].gameObject.transform.position) <= lightRenderDistance)
            {
                spotlights[i].gameObject.SetActive(true);
            }
            else
            {
                spotlights[i].gameObject.SetActive(false);
            }
        }
    }
}
