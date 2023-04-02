using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureCard : MonoBehaviour
{   
    [Header("Properties")]
    public string cardName;
    public float showTime;
    public float rotateSpeed;

    [Space(20)]
    [Header("DEBUG")]
    public bool canHide;
    public bool back;
    public bool canFlipAgain = true;
    public bool hasMatched;

    void Update()
    {
        if(transform.rotation.y == 0.1f)
            transform.rotation = Quaternion.Euler(0,0,0);
        transform.rotation = back? Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,180,0), Time.deltaTime*rotateSpeed) :
                                    Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,0), Time.deltaTime*rotateSpeed);
    }

    void OnMouseDown()
    {
        if(canFlipAgain && !MemoryPictureManager.Instance.hasFlipping)
        {
            GameManager.Instance.audioSource.PlayOneShot(GameManager.Instance.matchingFlipClip);
            if(MemoryPictureManager.Instance.AddToCheckList(this))
            {
                canFlipAgain = false;
                back = !back;
            } else
            {
                StartCoroutine(TimerFlipBack());
            }
        } 
    }

    public IEnumerator TimerFlipBack()
    {
        MemoryPictureManager.Instance.hasFlipping = true;
        canFlipAgain = false;
        yield return new WaitForSeconds(showTime);
        back = !back;
        canFlipAgain = true;
        MemoryPictureManager.Instance.hasFlipping = false;
    }
}
