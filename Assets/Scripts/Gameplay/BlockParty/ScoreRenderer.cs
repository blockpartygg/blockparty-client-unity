using UnityEngine;
using TMPro;
using System;

public class ScoreRenderer : MonoBehaviour {
    public Score Score;
    public TMP_Text Text;

    void Start() {
        UpdateText();
        Score.PointsChanged += HandlePointsChanged;
    }

    void HandlePointsChanged(object sender, EventArgs args) {
        UpdateText();
    }

    void UpdateText() {
        Text.text = Score.Points.ToString();
    }
}