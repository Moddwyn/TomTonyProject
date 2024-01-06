using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFix : MonoBehaviour
{
    void Awake() {
        GetComponent<TerrainCollider>().enabled = false;
        GetComponent<TerrainCollider>().enabled = true;
    }
}
