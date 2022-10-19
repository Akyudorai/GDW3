using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TMPro;

using System;
using System.Text;
using System.Runtime.InteropServices;

public class PauseMenu : MonoBehaviour
{
    //Reference: https://www.youtube.com/watch?v=tfzwyNS1LUY

    [DllImport("TestPlugin.dll")]
    private static extern IntPtr createTest();

    [DllImport("TestPlugin.dll")]
    private static extern void freeTest(IntPtr instance);

    [DllImport("TestPlugin.dll")]
    private static extern void GenerateTip(StringBuilder myString, int length);

    [SerializeField] GameObject pauseMenu;
    public TextMeshProUGUI tip;
    
    private void Start() 
    {
        GameManager.GetInstance().pcRef.inputAction.Player.Escape.performed += cntxt => Toggle();
    }

    private void Toggle() 
    {
        if (!pauseMenu.activeInHierarchy) { 
            Pause(); 
        } 
        
        else {
            Resume(); 
        }
    }

    public void Pause()
    {
        FreshTip();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    
    public void ButtonPress()
    {
        Debug.Log("Hello");
    }

    public void FreshTip()
    {
        IntPtr test = createTest();
        StringBuilder str = new StringBuilder(100);

        GenerateTip(str, str.Capacity);
        string myString = str.ToString();
        tip.text = myString;

        freeTest(test);
    }
}
