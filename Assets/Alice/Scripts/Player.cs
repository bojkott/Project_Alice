using UnityEngine;
using System.Collections;

public static class Player {
    public enum State { Grown, Shrunken, Growing, Shrinking };
    public static State currentState;
}
