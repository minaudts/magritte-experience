using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObject : MonoBehaviour
{
    private float hoverHeight = 0.02f;
    private float hoverSpeed = 2f;

    private float startY;
    private MeshRenderer _mesh;

    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
        //_mesh = GetComponent<MeshRenderer>();
    }

    public void InstantiateObject(GameObject faceObj, Material mat)
    {
        Instantiate(faceObj, transform);
        _mesh = GetComponentInChildren<MeshRenderer>();
        _mesh.material = mat;
    }

    private void Update() 
    {
        HoverUpAndDown();
    }

    private void HoverUpAndDown()
    {
        float newY = startY + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    
    public void SetMaterial(Material mat)
    {
        _mesh.material = mat;
    }
}
