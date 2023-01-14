using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public List<PictureItem> pictureItems = new List<PictureItem>();

    void Start()
    {
    }
}

[System.Serializable]
public class PictureItem
{
    public Sprite picture;
    public string word;
}