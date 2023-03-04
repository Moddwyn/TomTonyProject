using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaskeballManager : MonoBehaviour
{
    public Rigidbody currBall;
    public Transform currCam;
    public enum Position { LEFT, CENTER, RIGHT };
    public Position currPosition;

    [Space(20)]
    public Transform centerCam;
    public Rigidbody centerBall;
    public Transform rightCam;
    public Rigidbody rightBall;
    public Transform leftCam;
    public Rigidbody leftBall;
    [Space(20)]
    float force;
    public Color greenColor;
    public Color redColor;
    public float powerBarSpeed;
    public Vector2 minMaxPower;
    public Image powerBar;
    public float timeToReset;

    bool powering;
    bool groundHit;
    Vector3 orgPosCenter;
    Vector3 orgPosRight;
    Vector3 orgPosLeft;

    void Start()
    {
        
        orgPosCenter = centerBall.position;
        orgPosLeft = leftBall.position;
        orgPosRight = rightBall.position;
        UpdateBallPosition();
        powering = true;

    }

    void Update()
    {
        if(powering && (currBall != null) && (currCam != null))
        {
            powerBar.fillAmount = Mathf.InverseLerp(minMaxPower.x, minMaxPower.y, Mathf.Lerp(minMaxPower.x, minMaxPower.y, Mathf.PingPong(Time.time, powerBarSpeed)));
            powerBar.color = Color.Lerp(greenColor, redColor, Mathf.PingPong(Time.time, powerBarSpeed));

            if(Input.GetKeyDown(KeyCode.Space))
            {
                force = powerBar.fillAmount * minMaxPower.y;
                currBall.isKinematic = false;

                Vector3 forceDirection = currCam.forward;
                forceDirection *= force;
                currBall.AddForce(forceDirection / 8f + Vector3.up * forceDirection.y);
                powering = false;
            }
        }
    }

    void UpdateBallPosition()
    {
        leftBall.isKinematic = true;
        centerBall.isKinematic = true;
        rightBall.isKinematic = true;
        if(currPosition == Position.LEFT)
        {
            currBall = leftBall;
            currCam = leftCam;

        } else if(currPosition == Position.CENTER)
        {
            currBall = centerBall;
            currCam = centerCam;
        } else if(currPosition == Position.RIGHT)
        {
            currBall = rightBall;
            currCam = rightCam;
        }

        leftBall.GetComponent<MeshRenderer>().enabled = currPosition == Position.LEFT;
        leftCam.gameObject.SetActive(currPosition == Position.LEFT);

        rightBall.GetComponent<MeshRenderer>().enabled = currPosition == Position.RIGHT;
        rightCam.gameObject.SetActive(currPosition == Position.RIGHT);

        centerBall.GetComponent<MeshRenderer>().enabled = currPosition == Position.CENTER;
        centerCam.gameObject.SetActive(currPosition == Position.CENTER);

        leftBall.transform.position = orgPosLeft;
        centerBall.transform.position = orgPosCenter;
        rightBall.transform.position = orgPosRight;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ball" && !groundHit)
        {
            groundHit = true;
            StartCoroutine(ResetTime());
        }
    }

    IEnumerator ResetTime()
    {
        yield return new WaitForSeconds(timeToReset);
        int rand = Random.Range(0, 3);
        if(rand == 0) currPosition = Position.LEFT;
        else if(rand == 1) currPosition = Position.CENTER;
        else currPosition = Position.RIGHT;

        UpdateBallPosition();
        powering = true;
        groundHit = false;
    }
}
