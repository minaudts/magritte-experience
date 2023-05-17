using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObject : MonoBehaviour
{
    private float hoverHeight = 1f;
    private float hoverSpeed = 1f;

    private float startY;
    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
    }

    public void InstantiateObject(GameObject faceObj)
    {
        Instantiate(faceObj, transform);
    }

    private void Update() 
    {
        //HoverUpAndDown();
    }

    private void HoverUpAndDown()
    {
        float newY = startY + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    
}
