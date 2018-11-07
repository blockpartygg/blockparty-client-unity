using UnityEngine;

public enum BlockState {
    Empty,
    Idle,
    Sliding,
    WaitingToFall,
    Falling,
    Matched,
    WaitingToClear,
    Clearing,
    WaitingToEmpty
}

public class Block : MonoBehaviour {
    public BlockState State;

    public Vector2 Position;
    public int Type;
    public const int TypeCount = 6;
}