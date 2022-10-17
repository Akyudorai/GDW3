using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LampEvents
{
    public abstract Color LampColor();
    public abstract float LampIntensity();
}

public class Morning : LampEvents
{
    public override Color LampColor()
    {
        return new Color(255f / 255f, 255f / 255f, 255f / 255f);
    }

    public override float LampIntensity()
    {
        return 900000f;
    }
}

public class Afternoon : LampEvents
{
    public override Color LampColor()
    {
        return new Color(255f / 255f, 255f / 255f, 255f / 255f);
    }

    public override float LampIntensity()
    {
        return 0f;
    }
}

public class Night : LampEvents
{
    public override Color LampColor()
    {
        return new Color(243f / 255f, 197f / 255f, 109f / 255f);
    }

    public override float LampIntensity()
    {
        return 1000000f;
    }
}
