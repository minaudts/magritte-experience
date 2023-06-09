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

    public void InstantiateObject(GameObject faceObj, bool isSitting)
    {
        GameObject resObj = Instantiate(faceObj, transform);
        if(resObj.GetComponent<Key>())
        {
            resObj.transform.localPosition = new Vector3(-0.06f, 0.11f, 0.01f);
            resObj.transform.localEulerAngles = new Vector3(42.151f, 99.83f, 86.276f);
            resObj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            resObj.GetComponent<Key>().MakeCollectable(false);
            //GetComponentInParent<CrowdMan>().SetKey(resObj.GetComponent<Key>());
        }
        if (isSitting)
        {
            resObj.transform.localPosition = new Vector3(resObj.transform.localPosition.x, resObj.transform.localPosition.y + 0.095f, resObj.transform.localPosition.z);
        }
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
}
