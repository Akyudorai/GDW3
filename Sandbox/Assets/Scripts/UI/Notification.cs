using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public GameObject notificationIcon;
    public AnimationClip fadeIn;
    public AnimationClip fadeOut;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(NotificationSent());
        StartCoroutine(NotificationSentV2());
        Debug.Log(fadeIn.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator NotificationSent()
    {
        float currentTime = 0f;
        float maxTime = 10f;
        bool done = false;
        float _alpha = 255;
        
        while(done == false)
        {
            //Debug.Log("Working");
            if(currentTime <= maxTime)
            {
                currentTime += Time.deltaTime;
                _alpha = Mathf.Lerp(0f, 255f, currentTime / maxTime);
                Color newColor = notificationIcon.GetComponent<Image>().color;
                newColor.a = 55;
                notificationIcon.GetComponent<Image>().color = newColor;
                //Debug.Log(notificationIcon.GetComponent<Image>().color.a);
            }
            else
            {
                done = true;
            }
            yield return null;
        }        
    }

    IEnumerator NotificationSentV2()
    {
        Debug.Log("Fade In");
        notificationIcon.GetComponent<Animation>().Play(fadeIn.name);

        yield return new WaitForSeconds(3f);

        notificationIcon.GetComponent<Animation>().Play(fadeOut.name);
        Debug.Log("Fade Out");
    }
}
