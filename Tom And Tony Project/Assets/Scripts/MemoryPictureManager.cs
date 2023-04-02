using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class MemoryPictureManager : MonoBehaviour
{
    public UnityEvent OnWin;
    public UnityEvent OnMatch;
    
    public List<PictureCard> cardsTypes  = new List<PictureCard>();
    [ReadOnly] public List<PictureCard> matchCheckList = new List<PictureCard>();

    List<PictureCard> worldCards = new List<PictureCard>();
    List<PictureCard> newCards = new List<PictureCard>();
    List<int> remainIndex = new List<int>();
    [Space(20)]
    public Transform cardParent;

    Camera cam;
    bool won;
    [HideInInspector]
    public bool hasFlipping;

    public static MemoryPictureManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if(cardParent.childCount > 0)
        {
            for (int i = 0; i < cardParent.childCount; i++)
            {
                worldCards.Add(cardParent.GetChild(i).GetComponent<PictureCard>());
            }
        }
        if(worldCards.Count % 2 != 0)
            Debug.LogError("There's an odd amount of cards");

        LoadCards();
    }

    void Update()
    {
        MatchManager();

        if(newCards.All(x=>x.hasMatched && !won))
        {
            OnWin.Invoke();
            won = true;
        }
    }

    void MatchManager()
    {
        if(matchCheckList.Count == 2)
        {
            if(matchCheckList[0].cardName.Equals(matchCheckList[1].cardName))
            {
                OnMatch?.Invoke();
                matchCheckList[0].hasMatched = true;
                matchCheckList[1].hasMatched = true;
                matchCheckList.Clear();
            } else 
            {
                StartCoroutine(matchCheckList[0].TimerFlipBack());
                StartCoroutine(matchCheckList[1].TimerFlipBack());
                matchCheckList.Clear();
            }
        }
    }

    public bool AddToCheckList(PictureCard card)
    {
        if(matchCheckList.Count < 2)
        {
            matchCheckList.Add(card);
            return true;
        }
        return false;
    }
    
    public void LoadCards()
    {
        
        for (int i = 0; i < worldCards.Count; i++)
        {
            remainIndex.Add(i);
        }
        for (int i = 0; i < worldCards.Count/2; i++)
        {
            int randomType = Random.Range(0, cardsTypes.Count);
            for (int x = 0; x < 2; x++)
            {
                int randomRemain = Random.Range(0, remainIndex.Count);
                PictureCard cardInst = Instantiate(cardsTypes[randomType], worldCards[remainIndex[randomRemain]].transform.position, Quaternion.identity, cardParent);
                newCards.Add(cardInst);
                remainIndex.RemoveAt(randomRemain);
            }
        }

        for (int i = 0; i < worldCards.Count; i++)
        {
            worldCards[i].gameObject.SetActive(false);
        }
    }
}
