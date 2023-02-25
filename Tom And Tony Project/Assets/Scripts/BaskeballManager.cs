using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaskeballManager : MonoBehaviour
{
    public Rigidbody ball;
    public float forceMultipler;
    public float forwardForceReduction;
    private Vector3 mouseDownPos;
    private Vector3 mouseUpPos;

    void Start()
    {
        ball.isKinematic = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseUpPos = Input.mousePosition;

            // Calculate the velocity of the mouse movement
            float flickVelocity = (mouseUpPos - mouseDownPos).magnitude / Time.deltaTime;

            // Use the flick velocity for your needs
            Debug.Log("Flick velocity: " + flickVelocity);
            ball.isKinematic = false;

            flickVelocity *= forceMultipler;
            ball.AddForce(new Vector3((flickVelocity*forwardForceReduction)/100000, flickVelocity/100000, 0), ForceMode.Impulse);
        }

        if(Input.GetKeyDown(KeyCode.R)) GameManager.Instance.RestartScene();
    }

}
