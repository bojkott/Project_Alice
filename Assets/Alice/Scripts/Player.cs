using UnityEngine;
using System.Collections;

public static class Player {
    
    public enum Sizes { Giant, Normal, Midget}
    public enum State {Idle, Growing, Shrinking};
    public static int numberOfSizes = 3;
    public static Sizes currentSize = Sizes.Normal;
    public static State currentState = State.Idle;


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
}
