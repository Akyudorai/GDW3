using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LampEvents
{
    public class Day : LampEvents
    {
        public static Color GetColor()
        {
            return new Color(255f / 255f, 255f / 255f, 255f / 255f);
        }

        public static float GetIntensity()
        {
            return 0f;
        }
    }

    public class Night : LampEvents
    {
        public static Color GetColor()
        {
            return new Color(243f / 255f, 197f / 255f, 109f / 255f);
        }

        public static float GetIntensity()
        {
            return 1000000f;
        }
    }
}


