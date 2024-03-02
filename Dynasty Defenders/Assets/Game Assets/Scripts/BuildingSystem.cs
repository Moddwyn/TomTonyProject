using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSystem : MonoBehaviour
{
    public List<Tower> buildables = new List<Tower>();
    [ReadOnly] public int chosenBuild;
    [ReadOnly] public Tower currentBuilding;
    public Color chosenColor;
    public GameObject descriptionPanel;

    [HorizontalLine]
    public Camera cam;
    public float range = 7;
    public LayerMask buildableLayer;

    [HorizontalLine]
    public GameObject towerOptionUIPrefab;
    public Transform towerOptionList;

    [HorizontalLine]
    public TMP_Text crosshairStatus;
    public LayerMask towerLayer;

    public List<Image> towerOptionUIs = new List<Image>();

    void Start()
    {
        UpdateTowerList();
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;
        RaycastHit hit2;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(ray, out hit, range, buildableLayer))
            {
                PlaceBuild(hit.point);
            }
        }

        if (Physics.Raycast(ray, out hit2, range, towerLayer))
        {
            if(hit2.transform.root.TryGetComponent<Tower>(out Tower t))
            {
                crosshairStatus.text = "(R) Delete " + t.towerInfo.towerName +" Tower";
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Destroy(t.gameObject);
                }
            } else crosshairStatus.text = "";
            
        } else crosshairStatus.text = "";


        foreach (char c in Input.inputString)
        {
            if (int.TryParse(c.ToString(), out int num))
            {
                if (num >= 0 && num <= buildables.Count)
                {
                    chosenBuild = num - 1;
                    currentBuilding = buildables[chosenBuild];

                    foreach (var tou in towerOptionUIs) tou.color = Color.white;
                    towerOptionUIs[chosenBuild].color = chosenColor;
                }
            }
        }

        if(currentBuilding != null && Input.GetKey(KeyCode.Q))
        {
            descriptionPanel.SetActive(true);
            descriptionPanel.GetComponentInChildren<TMP_Text>().text = currentBuilding.towerInfo.description;
        } else descriptionPanel.SetActive(false);
    }

    void PlaceBuild(Vector3 position)
    {
        if (currentBuilding != null && currentBuilding.towerInfo.cost <= CoinsManager.Instance.coins)
        {
            CoinsManager.Instance.ChangeCoins(-currentBuilding.towerInfo.cost);
            Instantiate(currentBuilding, position, Quaternion.identity);
        }
    }

    void UpdateTowerList()
    {
        for (int i = 0; i < buildables.Count; i++)
        {
            Tower currTower = buildables[i];
            Transform newOption = Instantiate(towerOptionUIPrefab, towerOptionList).transform;
            newOption.GetChild(0).GetComponent<TMP_Text>().text = i + 1 + "";
            newOption.GetChild(0).GetChild(0).GetComponent<Image>().sprite = currTower.towerInfo.icon;
            newOption.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = currTower.towerInfo.cost + "";

            towerOptionUIs.Add(newOption.GetComponent<Image>());
        }
    }
}
