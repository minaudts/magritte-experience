using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeashooterTest : MonoBehaviour
{
    private Vector3 pos1;
    public Vector3 pos2;
    private Vector3 tempPos;
    public float moveSpeed;
    public float waitTime;

    bool canStartLoop;
    bool startRotation;
    bool rotationState;
    public Vector3 rotationSpeed;

    public Animator animator;

    void Start()
    {
        pos1 = gameObject.transform.position;
        pos2.y = gameObject.transform.position.y;

        moveSpeed *= Time.deltaTime;

        canStartLoop = true;
    }
    private void FixedUpdate()
    {        
        if (canStartLoop)
        {
            StartCoroutine("PeashooterCycle");
        }
    }
    void Update()
    {
        Debug.DrawLine(pos1, pos2, Color.red, Mathf.Infinity);

        if (startRotation)
            Rotate180();              
    }

    private IEnumerator PeashooterCycle()
    {
        canStartLoop = false;
        //wanneer peashooter van target verwijderd is
        if (Vector3.Distance(transform.position, pos2) > .1f)
        {
            animator.SetBool("isJumping", true);
            transform.position = Vector3.MoveTowards(transform.position, pos2, moveSpeed);
        }

        //wanneer peashooter target bereikt heeft
        if (Vector3.Distance(transform.position, pos2) < .1f)
        {
            animator.SetBool("isJumping", false);
            animator.SetTrigger("lookAround");
            yield return new WaitForSeconds(waitTime);
            //startRotation = true;
            tempPos = pos1;
            pos1 = pos2;
            pos2 = tempPos;
        }           
        canStartLoop = true;
    }

    void Rotate180()
    {
        if (!rotationState)
        {
            if (transform.rotation.eulerAngles.y <= 180)
            {
                transform.eulerAngles += rotationSpeed;
            }
            if (transform.rotation.eulerAngles.y >= 180)
            {
                rotationState = true;
                startRotation = false;
            }
        } else
        {
            if(transform.rotation.eulerAngles.y >= 0)
            {
                transform.eulerAngles -= rotationSpeed;
            }
            if(transform.rotation.eulerAngles.y <= 0)
            {
                rotationState = false;
                startRotation = false;
            }
        }
        
    }
}
