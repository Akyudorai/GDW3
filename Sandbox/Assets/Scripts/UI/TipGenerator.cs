using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using System;
using System.Text;
using System.Runtime.InteropServices;

/*
 * Script for generating gameplay tips. Uses a custom made plugin and is currently used in conjunction
 * with the tip generator app on the phone screen.
 */
public class TipGenerator : MonoBehaviour
{
    //Reference: https://www.youtube.com/watch?v=tfzwyNS1LUY

    [DllImport("TestPlugin.dll")]
    private static extern IntPtr createTest();

    [DllImport("TestPlugin.dll")]
    private static extern void freeTest(IntPtr instance);

    [DllImport("TestPlugin.dll")]
    private static extern void GenerateTip(StringBuilder myString, int length);

    public TextMeshProUGUI tip;

    public List<String> freeTips;

    public void FreshTip()
    {
        //IntPtr test = createTest();
        //StringBuilder str = new StringBuilder(100);

        //GenerateTip(str, str.Capacity);
        //string myString = str.ToString();
        //tip.text = myString;

        //freeTest(test);

        tip.text = UI_Manager.GetInstance().CreateNewTip();
    }
}
