using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class MazeGoal : MonoBehaviour
{
    public bool finished;
    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
            finished = true;
    }
}
