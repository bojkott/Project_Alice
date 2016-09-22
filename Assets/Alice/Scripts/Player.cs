using UnityEngine;
using System.Collections;

public static class Player {
    
    public enum Sizes { Giant, Normal, Midget}
    public enum State {Idle, Growing, Shrinking};
    public static int numberOfSizes = 3;
    public static Sizes currentSize = Sizes.Normal;
    public static State currentState = State.Idle;

    public static float giantSize = 100.0f;
    public static float normalSize = 10.0f;
    public static float midgetSize = 1.0f;


    public static int getSizeIndex(Sizes size)
    {
        if (size == Player.Sizes.Giant)
            return 0;
        else if (size == Player.Sizes.Normal)
            return 1;
        else if (size == Player.Sizes.Midget)
            return 2;

        return 0;
    }


    public static bool CanGrow()
    {
        return getSizeIndex(currentSize) > 0;
    }

    public static bool CanShrink()
    {
        return getSizeIndex(currentSize) < numberOfSizes - 1;
    }


    public static float GetSizeScale(Player.Sizes size)
    {
        if (size == Player.Sizes.Giant)
            return giantSize;
        else if (size == Player.Sizes.Normal)
            return normalSize;
        else if (size == Player.Sizes.Midget)
            return midgetSize;

        return 0;
    }
}
