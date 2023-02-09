using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public Sprite characterProfile; //the background image
    public Sprite characterCard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract()
    {
        CharacterSelectionLogic.GetInstance().SetBackground(characterProfile);
        //CharacterSelectionLogic.GetInstance().selectedCharacter.GetComponent<Image>().sprite = characterCard;
    }
}
