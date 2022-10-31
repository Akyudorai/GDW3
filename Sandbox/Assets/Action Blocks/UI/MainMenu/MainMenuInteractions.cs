using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuInteractions : MonoBehaviour
{
    //public GameObject RunningSprite;
    public AnimationClip buttonSlideAnim;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(buttonSlideAnim.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MouseEnter()
    {
        GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Italic;
        //if(RunningSprite != null)
        //{
        //    RunningSprite.SetActive(true);
        //}
        
    }
    
    public void MouseExit()
    {
        GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        //RunningSprite.SetActive(false);
    }

    public void ButtonPressed()
    {
        //GetComponent<Transform>().position = new Vector3((transform.position.x + Camera.main.orthographicSize * 2f), (transform.position.x + Camera.main.orthographicSize * 2f), 0f);
        MoveButton(this.gameObject);
    }

    void MoveButton(GameObject _button)
    {
        Camera camera = Camera.main.GetComponent<Camera>();
        Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Gizmos.color = Color.yellow;
        _button.GetComponent<RectTransform>().position = p;
    }
}
