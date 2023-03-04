using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaskeballManager : MonoBehaviour
{
    public Rigidbody ball;
    float force;
    public Color greenColor;
    public Color redColor;
    public float powerBarSpeed;
    public Vector2 minMaxPower;
    public Image powerBar;
    public float timeToReset;

    bool powering;
    bool groundHit;
    Vector3 orgPos;

    void Start()
    {
        ball.isKinematic = true;
        powering = true;
        orgPos = ball.transform.position;
    }

    void Update()
    {
        if(powering)
        {
            powerBar.fillAmount = Mathf.InverseLerp(minMaxPower.x, minMaxPower.y, Mathf.Lerp(minMaxPower.x, minMaxPower.y, Mathf.PingPong(Time.time, powerBarSpeed)));
            powerBar.color = Color.Lerp(greenColor, redColor, Mathf.PingPong(Time.time, powerBarSpeed));

            if(Input.GetKeyDown(KeyCode.Space))
            {
                force = powerBar.fillAmount * minMaxPower.y;
                ball.isKinematic = false;
                ball.AddForce(new Vector3(force/3, force, 0));
                powering = false;
            }
        }
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
        ball.isKinematic = true;
        ball.transform.position = orgPos;
        powering = true;
        groundHit = false;
    }
}
