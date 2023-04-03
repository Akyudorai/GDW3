using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpUi : MonoBehaviour
{
    public Sprite tutorialImage; //used for the help screen background;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = this.gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTutorial()
    {
        UI_Manager.GetInstance().HelpPanel.GetComponent<Image>().sprite = tutorialImage;
        UI_Manager.GetInstance().helpList.SetActive(false);
        UI_Manager.GetInstance().viewingTutorial = true;
    }
}
