using UnityEngine;
using System;

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
    BlockState state;
    public BlockState State {
        get { return state; }
        set { 
            if(state != value) {
                state = value;
                if(StateChanged != null) {
                    StateChanged(this, null);
                }
            }
        }
    }
    public int Column, Row;
    int type;
    public int Type {
        get { return type; }
        set {
            if(type != value) {
                type = value;
                if(TypeChanged != null) {
                    TypeChanged(this, null);
                }
            }
        }
    }
    public const int TypeCount = 6;
    public BlockSlider Slider;
    public BlockFaller Faller;
    public BlockMatcher Matcher;
    public BlockClearer Clearer;
    public BlockEmptier Emptier;
    public event EventHandler StateChanged;
    public event EventHandler TypeChanged;
}