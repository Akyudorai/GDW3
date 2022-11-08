using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public GameObject[] characters;
    public int currentSelection = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(characters.Length > 0)
        {
            characters[0].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextCharacter()
    {
        characters[currentSelection].SetActive(false);
        currentSelection += 1;
        if(currentSelection >= characters.Length)
        {
            currentSelection = 0;
        }
        characters[currentSelection].SetActive(true);
    }

    public void PreviousCharacter()
    {
        characters[currentSelection].SetActive(false);
        currentSelection -= 1;
        
        if(currentSelection < 0)
        {
            currentSelection = characters.Length - 1;
        }
        Debug.Log(currentSelection);
        characters[currentSelection].SetActive(true);
    }

}
