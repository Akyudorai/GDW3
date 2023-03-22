using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// -- TODO -- Convert this class into a localized singleton to allow other classes to easily activate cinematics for the given scene
// -- TODO -- Create a cinematic loader class to place in each scene to update the localized cinematic paths and settings

public class CinematicFollow : MonoBehaviour
{
    // -- TODO -- Get reference to cinematic camera and set it to start position and rotation upon beginning play
    public GameObject MainCam;
    public GameObject CinematicCam;
    public PlayableDirector director; 
    public GameObject cinematicObject = null;

    // -- TODO -- Delay the race start until after the completion of the cinematic sequence
    // -- TODO -- Add a second cinematic sequence where the camera twirls around the player and ends at their back from the standard controller point of view during countdown sequence of race
    public void Play() 
    {
        if (cinematicObject != null) {
            cinematicObject.SetActive(true);

            Debug.Log("Duration: " + director.duration);
            MainCam.SetActive(false);
            director.Play();
            StartCoroutine(DisableOnFinish());
        }
    }

    private IEnumerator DisableOnFinish() 
    {
        yield return new WaitForSeconds((float)director.duration);
        cinematicObject.SetActive(false);
        MainCam.SetActive(true);
    }

}
