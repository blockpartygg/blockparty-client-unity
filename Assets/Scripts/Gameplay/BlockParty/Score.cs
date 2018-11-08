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

    public void ScoreMatch() {
        Points += matchValue;
    }

    public void SubmitMatch(int matchedBlockCount) {
        if(AuthenticationManager.Instance.CurrentUser != null) {
            SocketManager.Instance.Socket.Emit("blockParty/scoreMatch", AuthenticationManager.Instance.CurrentUser.UserId, matchedBlockCount);
        }
    }
}