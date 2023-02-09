using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionLogic : MonoBehaviour
{
    // Singleton Instance
    private static CharacterSelectionLogic instance;
    public static CharacterSelectionLogic GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }

    public Image _background;
    public GameObject selectedCharacter;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public void SetBackground(Sprite _backgroundSprite)
    {
        _background.sprite = _backgroundSprite;
    }

}
