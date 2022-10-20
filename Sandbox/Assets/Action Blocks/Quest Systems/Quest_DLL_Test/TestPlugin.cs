using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Text;
using System.Runtime.InteropServices;

using TMPro;


public class TestPlugin : MonoBehaviour
{
    [DllImport("TestPlugin.dll")]
    private static extern IntPtr createTest();

    [DllImport("TestPlugin.dll")]
    private static extern void freeTest(IntPtr instance);

    [DllImport("TestPlugin.dll")]
    private static extern int getResult(IntPtr instance);

    [DllImport("TestPlugin.dll")]
    private static extern void setDialogTitle(string title);

    [DllImport("TestPlugin.dll")]
    private static extern void getHello(char _char);

    [DllImport("TestPlugin.dll")]
    private static extern void FillString(StringBuilder myString, int length);

    [DllImport("TestPlugin.dll")]
    private static extern void GenerateTip(StringBuilder myString, int length);

    public TextMeshProUGUI tip;

    private void Start()
    {
        IntPtr test = createTest();
        //Debug.Log(test);

        int result = getResult(test);
        //Debug.Log(result);

        StringBuilder str = new StringBuilder(100);
                   
        GenerateTip(str, str.Capacity);
        string myString = str.ToString();
        //tip.text = myString;               

        freeTest(test);
    }
}
