using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour
{
    public List<Sprite> icons;
    public GameObject cardPrefab;
    public GameObject grid;

    public GameObject card1 = null;
    public GameObject card2 = null;
    public bool card1Flipped = false;
    public bool card2Flipped = false;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 16; i++)
        {
            GameObject card = GameObject.Instantiate(cardPrefab, grid.transform);
            card.name = "card" + i;
            //Debug.Log(i);
            int num = Random.Range(0, icons.Count);
            card.GetComponent<MatchCard>().icon.GetComponent<Image>().sprite = icons[num];
            icons.RemoveAt(num);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCard(GameObject card)
    {
        if(card1Flipped == false)
        {
            card1 = card;
            card1Flipped = true;
        }
        else if(card2Flipped == false)
        {
            card2 = card;
            card2Flipped = true;
            CheckMatch();
        }
        else
        {
            
        }
    }

    public void CheckMatch()
    {
        Debug.Log("Cards are full");
        if (card1.GetComponent<MatchCard>().icon.GetComponent<Image>().sprite == card2.GetComponent<MatchCard>().icon.GetComponent<Image>().sprite)
        {
            Debug.Log("Match");
            Destroy(card1);
            Destroy(card2);
        }
        else
        {
            Debug.Log("No Match");
            card1.GetComponent<MatchCard>().UnFlipCard();
            card2.GetComponent<MatchCard>().UnFlipCard();
            card1Flipped = false;
            card2Flipped = false;
        }
    }
}
