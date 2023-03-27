using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings
{   
    // 1 = Nyx, 2 = Bea
    public int Character = 1;
}

public class PlayerIdentity
{
    public static NetworkIdentity NetID;
    public static string Username = "";

    public static PlayerSettings Settings = new PlayerSettings();
}
