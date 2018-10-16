using UnityEngine;
using TMPro;

public class GameStateRenderer : MonoBehaviour {
	TMP_Text gameStateText;

	void Awake() {
		gameStateText = GetComponent<TMP_Text>();
	}

	void Update() {
		switch(Game.Instance.State) {
			case Game.GameState.PregameCountdown:
			case Game.GameState.PregameTitle:
			case Game.GameState.PregameIntroduction:
				gameStateText.text = "The party starts soon";
				break;
			case Game.GameState.RoundIntroduction:
			case Game.GameState.RoundInstructions:
				gameStateText.text = "Round " + Game.Instance.Round + " starts soon";
				break;
			case Game.GameState.MinigameStart:
			case Game.GameState.MinigamePlay:
			case Game.GameState.MinigameEnd:
				gameStateText.text = "Round " + Game.Instance.Round + " is in progress";
				break;
			case Game.GameState.RoundResultsScoreboard:
			case Game.GameState.RoundResultsLeaderboard:
				gameStateText.text = "Round " + Game.Instance.Round + " just ended";
				break;
			case Game.GameState.PostgameCelebration:
			case Game.GameState.PostgameRewards:
				gameStateText.text = "The party just ended";
				break;
		}
	}
}
