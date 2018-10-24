using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : Singleton<SceneNavigator> {
	bool isPlaying;
	bool isLoadingScene;

	public void Navigate(string sceneName) {
		StartCoroutine(LoadSceneAsync(sceneName));
	}

	IEnumerator LoadSceneAsync(string sceneName) {
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

		while(!asyncLoad.isDone) {
			yield return null;
		}

		isLoadingScene = false;
	}

	public void StartPlaying() {
		isPlaying = true;
	}

	public void StopPlaying() {
		isPlaying = false;
		Navigate("HomeScene");
	}

	void Update() {
		string sceneToLoad = "";
		if(isPlaying) {
			switch(GameManager.Instance.State) {
				case GameManager.GameState.PregameCountdown:
					sceneToLoad = "PregameCountdownScene";
					break;
				case GameManager.GameState.PregameTitle:
					sceneToLoad = "PregameTitleScene";
					break;
				case GameManager.GameState.RoundIntroduction:
					sceneToLoad = "RoundIntroductionScene";
					break;
				case GameManager.GameState.MinigameStart:
				case GameManager.GameState.MinigamePlay:
				case GameManager.GameState.MinigameEnd:
					switch(GameManager.Instance.Minigame.Id) {
						case "redLightGreenLight":
						case "blockBlaster":
						case "blockio":
							sceneToLoad = "RedLightGreenLightScene";
							break;
					}
					break;
				case GameManager.GameState.RoundResultsScoreboard:
					sceneToLoad = "RoundResultsScoreboardScene";
					break;
				case GameManager.GameState.RoundResultsLeaderboard:
					sceneToLoad = "RoundResultsLeaderboardScene";
					break;
				case GameManager.GameState.PostgameCelebration:
					sceneToLoad = "PostgameCelebrationScene";
					break;
				case GameManager.GameState.PostgameRewards:
					sceneToLoad = "PostgameRewardsScene";
					break;
			}

			if(SceneManager.GetActiveScene().name != sceneToLoad && !isLoadingScene) {
				isLoadingScene = true;
				Navigate(sceneToLoad);
			}
		}
	}
}
