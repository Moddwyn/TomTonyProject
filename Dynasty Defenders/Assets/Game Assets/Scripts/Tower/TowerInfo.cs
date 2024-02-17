using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu()]
public class TowerInfo : ScriptableObject
{
    public string towerName;
    [Multiline] public string description;
    public int cost;
    [ShowAssetPreview] public Sprite icon;

    [HorizontalLine]
    public AudioClip OnPlaceAudio;
    public AudioClip OnFireAudio;

    [HorizontalLine]
    public List<TowerUpgrade> upgrades = new List<TowerUpgrade>();
}

[System.Serializable]
public class TowerUpgrade
{
    public string upgradeName;
    public int cost;
    [Multiline] public string description;
}
