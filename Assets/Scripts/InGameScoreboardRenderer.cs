using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScoreboardRenderer : MonoBehaviour {
	public GameObject ScoreboardContent;
	public InGameScoreboardEntry PlayerScore;
	public InGameScoreboardEntry InGameScoreboardEntryPrefab;
	void Awake() {
		GameManager.Instance.ScoreboardChanged += HandleScoreboardChanged;
	}

	void OnDestroy() {
		GameManager.Instance.ScoreboardChanged -= HandleScoreboardChanged;
	}

	void HandleScoreboardChanged(object sender, EventArgs args) {
		// Fetch the descending sorted scoreboard
		List<KeyValuePair<string, long>> sortedScoreboard = new List<KeyValuePair<string, long>>(GameManager.Instance.Scoreboard);
		sortedScoreboard.Sort(delegate(KeyValuePair<string, long> firstPair, KeyValuePair<string, long> secondPair) {
			return firstPair.Value.CompareTo(secondPair.Value);
		});
		sortedScoreboard.Reverse();

		// Clear out the existing scoreboard UI (this seems inefficient)
		for(int childIndex = 0; childIndex < ScoreboardContent.transform.childCount; childIndex++) {
			Destroy(ScoreboardContent.transform.GetChild(childIndex).gameObject);
		}

		// Populate the scoreboard UI
		int rank = 1;
		foreach(KeyValuePair<string, long> score in sortedScoreboard) {
			// Set the player score if we come across the authenticated player ID
			if(AuthenticationManager.Instance.CurrentUser != null) {
				if(GameManager.Instance.Mode.Id == "freeForAll") {
					if(score.Key == AuthenticationManager.Instance.CurrentUser.UserId) {
						PlayerScore.RankText.text = rank.ToString();
						PlayerScore.NameText.text = PlayerManager.Instance.Players.ContainsKey(score.Key) ? PlayerManager.Instance.Players[score.Key].name : "Loading...";
						PlayerScore.ScoreText.text = score.Value.ToString();
					}
				}
				else if(GameManager.Instance.Mode.Id == "redVsBlue") {
					if(GameManager.Instance.Teams.ContainsKey("redTeamId") && GameManager.Instance.Teams["redTeamId"].Contains(AuthenticationManager.Instance.CurrentUser.UserId)) {
						PlayerScore.RankText.text = rank.ToString();
						PlayerScore.NameText.text = PlayerManager.Instance.Players["redTeamId"].name;
						PlayerScore.ScoreText.text = score.Value.ToString();
					}
					else if(GameManager.Instance.Teams.ContainsKey("blueTeamId") && GameManager.Instance.Teams["blueTeamId"].Contains(AuthenticationManager.Instance.CurrentUser.UserId)) {
						PlayerScore.RankText.text = rank.ToString();
						PlayerScore.NameText.text = PlayerManager.Instance.Players["redTeamId"].name;
						PlayerScore.ScoreText.text = score.Value.ToString();
					}
					else {
						PlayerScore.RankText.text = "-";
						PlayerScore.NameText.text = "Spectating";
						PlayerScore.ScoreText.text = "-";
					}
				}
			}

			InGameScoreboardEntry scoreObject = Instantiate(InGameScoreboardEntryPrefab, Vector3.zero, Quaternion.identity);
			scoreObject.RankText.text = (rank++).ToString();
			scoreObject.NameText.text = PlayerManager.Instance.Players.ContainsKey(score.Key) ? PlayerManager.Instance.Players[score.Key].name : "Loading...";
			scoreObject.ScoreText.text = score.Value.ToString();
			scoreObject.transform.SetParent(ScoreboardContent.transform);
			scoreObject.transform.localScale = Vector3.one;
		}
	}
}
