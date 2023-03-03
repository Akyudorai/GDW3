using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://alessandrofama.com/tutorials/fmod/unity/footsteps

public class Footsteps : MonoBehaviour
{
    private PlayerController playerController;
    float timer = 0f;
    [SerializeField]
    float footstepSpeed = 0.3f;
    bool footStepsPlaying = false;

    private enum CURRENT_TERRAIN { GROUND, ROAD};   

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;
    private FMOD.Studio.EventInstance footSteps;
    // Start is called before the first frame update
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        footSteps = FMODUnity.RuntimeManager.CreateInstance("event:/Running");
        footSteps.setParameterByName("Terrain", 0);
        footSteps.start();
        footSteps.setPaused(true);
    }

    // Update is called once per frame
    void Update()
    {
        DetermineTerrain();

        if(playerController.v_HorizontalVelocity.magnitude > 2f && playerController.IsGrounded())
        {
            //if(timer > footstepSpeed)
            //{
            //    SelectAndPlayFootStep();
            //    timer = 0.0f;
            //}
            //timer += Time.deltaTime;
            if(footStepsPlaying == false)
            {

                footSteps.setParameterByName("Terrain", 0);
                footSteps.setPaused(false);
                footStepsPlaying = true;
            }            
        }
        else
        {
            if(footStepsPlaying == true)
            {
                //footSteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                //footSteps.release();
                footSteps.setPaused(true);
                footStepsPlaying = false;
            }
            
        }
        footSteps.setPitch(remapRange(playerController.v_HorizontalVelocity.magnitude, 0f, 15.5f, 0f, 2f));
        //Debug.Log(remapRange(playerController.v_HorizontalVelocity.magnitude, 0f, 15.5f, 0f, 2f));
    }

    private void DetermineTerrain()
    {
        RaycastHit[] hit;

        hit = Physics.RaycastAll(transform.position, Vector3.down, 10.0f);

        foreach (RaycastHit rayhit in hit)
        {
            if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                currentTerrain = CURRENT_TERRAIN.GROUND;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Road"))
            {
                currentTerrain = CURRENT_TERRAIN.ROAD;
            }
        }
    }

    private void PlayFootSteps(int terrain)
    {
        footSteps = FMODUnity.RuntimeManager.CreateInstance("event:/Running");
        footSteps.setParameterByName("Terrain", terrain);
        footSteps.start();
        footSteps.release();
    }

    public void SelectAndPlayFootStep()
    {
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.GROUND:
                PlayFootSteps(0);
                break;
            case CURRENT_TERRAIN.ROAD:
                PlayFootSteps(1);
                break;

            default:
                PlayFootSteps(0);
                break;
        }
    }

    public float remapRange(float targetNum, float a1, float a2, float b1, float b2)
    {
        return b1 + (targetNum - a1) * (b2 - b1) / (a2 - a1);
    }
}
