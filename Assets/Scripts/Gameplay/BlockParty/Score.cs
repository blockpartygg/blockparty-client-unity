using UnityEngine;
using System;

public class Score : MonoBehaviour {
    int points;
    public event EventHandler PointsChanged;
    public int Points {
        get { return points; }
        set {
            if(points != value) {
                points = value;
                if(PointsChanged != null) {
                    PointsChanged(this, null);
                }
            }
        }
    }
    const int matchValue = 10;
    readonly int[] comboValue = new int[] { 0, 0, 0, 20, 30, 50, 60, 70, 80, 100, 140, 170, 210, 250, 290, 340, 390, 440, 490, 550, 610, 680, 750, 820, 900, 980, 1060, 1150, 1240, 1330 };
    readonly int[] chainValue = new int[] { 0, 50, 80, 150, 300, 400, 500, 700, 900, 1100, 1300, 1500, 1800 };
    const int raiseValue = 1;

    public void ScoreMatch() {
        Points += matchValue;
    }

    public void SubmitMatch(int matchedBlockCount) {
        int points = matchedBlockCount * matchValue;
        if(AuthenticationManager.Instance.CurrentUser != null && SocketManager.Instance.IsConnected) {
            SocketManager.Instance.Socket.Emit("blockParty/scorePoints", AuthenticationManager.Instance.CurrentUser.UserId, points);
        }
    }

    public void ScoreCombo(int matchedBlockCount) {
        int points = comboValue[Math.Min(matchedBlockCount - 1, comboValue.Length - 1)];
        Points += points;
        if(AuthenticationManager.Instance.CurrentUser != null && SocketManager.Instance.IsConnected) {
            SocketManager.Instance.Socket.Emit("blockParty/scorePoints", AuthenticationManager.Instance.CurrentUser.UserId, points);
        }
    }

    public void ScoreChain(int chainLength) {
        int points = chainValue[Math.Min(chainLength - 1, chainValue.Length - 1)];
        Points += points;
        if(AuthenticationManager.Instance.CurrentUser != null && SocketManager.Instance.IsConnected) {
            SocketManager.Instance.Socket.Emit("blockParty/scorePoints", AuthenticationManager.Instance.CurrentUser.UserId, points);
        }
    }

    public void ScoreRaise() {
        int points = raiseValue;
        Points += points;
        if(AuthenticationManager.Instance.CurrentUser != null && SocketManager.Instance.IsConnected) {
            SocketManager.Instance.Socket.Emit("blockParty/scorePoints", AuthenticationManager.Instance.CurrentUser.UserId, points);
        }
    }
}