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
    WaitingToEmpty,
}

public class Block : MonoBehaviour {
    [SerializeField]
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

    [SerializeField]
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
    public const int TypeCount = 5; // Empty = -1, Garbage = 5

    public BlockGarbage Garbage;
    public BlockSlider Slider;
    public BlockFaller Faller;
    public BlockMatcher Matcher;
    public BlockClearer Clearer;
    public BlockEmptier Emptier;
    public BlockChainer Chainer;
    public BlockKiller Killer;
    public event EventHandler StateChanged;
    public event EventHandler TypeChanged;
}