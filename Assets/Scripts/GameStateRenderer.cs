﻿using UnityEngine;
using TMPro;

public class GameStateRenderer : MonoBehaviour {
	TMP_Text gameStateText;

	void Awake() {
		gameStateText = GetComponent<TMP_Text>();
	}

	void Update() {
		switch(GameManager.Instance.State) {
			case GameManager.GameState.PregameCountdown:
			case GameManager.GameState.PregameTitle:
			case GameManager.GameState.PregameIntroduction:
				gameStateText.text = "The party starts soon";
				break;
			case GameManager.GameState.RoundIntroduction:
			case GameManager.GameState.RoundInstructions:
				gameStateText.text = "Round " + GameManager.Instance.Round + " starts soon";
				break;
			case GameManager.GameState.MinigameStart:
			case GameManager.GameState.MinigamePlay:
			case GameManager.GameState.MinigameEnd:
				gameStateText.text = "Round " + GameManager.Instance.Round + " is in progress";
				break;
			case GameManager.GameState.RoundResultsScoreboard:
			case GameManager.GameState.RoundResultsLeaderboard:
				gameStateText.text = "Round " + GameManager.Instance.Round + " just ended";
				break;
			case GameManager.GameState.PostgameCelebration:
			case GameManager.GameState.PostgameRewards:
				gameStateText.text = "The party just ended";
				break;
		}
	}
}
