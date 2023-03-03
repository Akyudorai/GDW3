using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaunchDirection 
{
    Forward,
    Up
}

public class Launcher : MonoBehaviour
{
    public float LaunchForce = 15.0f;
    public bool CanLaunch = true;
    public LaunchDirection Direction = LaunchDirection.Forward;

    private IEnumerator LaunchpadDelay() 
    {
        CanLaunch = false;
        yield return new WaitForSeconds(0.1f);
        CanLaunch = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && CanLaunch) 
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.v_VerticalVelocity = Vector3.zero;
            
            Vector3 direction = Vector3.zero;
            switch (Direction) {
                case LaunchDirection.Forward:
                    direction = transform.forward;
                    break;
                case LaunchDirection.Up:
                    direction = Vector3.up;
                    break;
            }
            Vector3 launchVector = direction * LaunchForce;            
            pc.ApplyForce(launchVector, ForceMode.Impulse);            
            //StartCoroutine(pc.OverrideMovement(2.0f));
            StartCoroutine(LaunchpadDelay());
            StartCoroutine(pc.JumpDelay());

            // Play JumpPad SFX
            FMOD.Studio.EventInstance jumpPadSFX = SoundManager.CreateSoundInstance(SoundFile.JumpPad);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(jumpPadSFX, other.gameObject.transform, GameManager.GetInstance().pcRef.rigid);
            jumpPadSFX.start();
            jumpPadSFX.release(); 
        }
    }
}
