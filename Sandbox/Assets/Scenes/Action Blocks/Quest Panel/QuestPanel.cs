using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestPanel : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questObjecive;
    public Image questImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateQuest(string _title, string _description, string _objective, Image _image) //am I even using this?
    {
        questTitle.text = _title;
        questDescription.text = _description;
        questObjecive.text = _objective;
        questImage.GetComponent<Image>().sprite = _image.GetComponent<Image>().sprite;
    }
}
