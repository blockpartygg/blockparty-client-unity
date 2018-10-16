using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : Singleton<SceneNavigator> {
	bool isPlaying;

	public void Navigate(string sceneName) {
		StartCoroutine(LoadSceneAsync(sceneName));
	}

	IEnumerator LoadSceneAsync(string sceneName) {
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

		while(!asyncLoad.isDone) {
			yield return null;
		}
	}

	public void StartPlaying() {
		isPlaying = true;
	}

	public void StopPlaying() {
		isPlaying = false;
	}

	void Update() {
		if(isPlaying) {
			string sceneToLoad = "";
			switch(Game.Instance.State) {
				case Game.GameState.PregameCountdown:
					sceneToLoad = "PregameCountdownScene";
					break;
				case Game.GameState.PregameTitle:
					sceneToLoad = "PregameTitleScene";
					break;
				case Game.GameState.PregameIntroduction:
					sceneToLoad = "PregameIntroductionScene";
					break;
				case Game.GameState.RoundIntroduction:
					sceneToLoad = "RoundIntroductionScene";
					break;
				case Game.GameState.RoundInstructions:
					sceneToLoad = "RoundInstructionsScene";
					break;
				case Game.GameState.MinigameStart:
				case Game.GameState.MinigamePlay:
				case Game.GameState.MinigameEnd:
					switch(Game.Instance.Minigame.Id) {
						case "redLightGreenLight":
						case "blockBlaster":
						case "blockio":
							sceneToLoad = "RedLightGreenLightScene";
							break;
					}
					break;
				case Game.GameState.RoundResultsScoreboard:
					sceneToLoad = "RoundResultsScoreboardScene";
					break;
				case Game.GameState.RoundResultsLeaderboard:
					sceneToLoad = "RoundResultsLeaderboardScene";
					break;
				case Game.GameState.PostgameCelebration:
					sceneToLoad = "PostgameCelebrationScene";
					break;
				case Game.GameState.PostgameRewards:
					sceneToLoad = "PostgameRewardsScene";
					break;
			}
			if(SceneManager.GetActiveScene().name != sceneToLoad) {
				Navigate(sceneToLoad);
			}
		}
		
	}
}
