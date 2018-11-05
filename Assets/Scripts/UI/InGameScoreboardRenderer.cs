using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScoreboardRenderer : MonoBehaviour {
	List<InGameScoreboardEntry> scoreboardEntries;
	public GameObject ScoreboardContent;
	public InGameScoreboardEntry PlayerScore;
	public InGameScoreboardEntry InGameScoreboardEntryPrefab;
	
	void Awake() {
		// Instantiate a scoreboard entry for every playing player
		scoreboardEntries = new List<InGameScoreboardEntry>();
		int rank = 1;
		foreach(KeyValuePair<string, Player> player in PlayerManager.Instance.Players) {
			if(player.Value.playing) {
				InGameScoreboardEntry scoreboardEntry = Instantiate(InGameScoreboardEntryPrefab, Vector3.zero, Quaternion.identity);
				scoreboardEntry.RankText.text = (rank++).ToString();
				scoreboardEntry.transform.SetParent(ScoreboardContent.transform);
				scoreboardEntry.transform.localScale = Vector3.one;
				scoreboardEntries.Add(scoreboardEntry);
			}
		}

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

		// Populate the scoreboard UI
		int rank = 1;
		int index = 0;
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

			scoreboardEntries[index].RankText.text = (rank++).ToString();
			scoreboardEntries[index].NameText.text = PlayerManager.Instance.Players.ContainsKey(score.Key) ? PlayerManager.Instance.Players[score.Key].name : "Loading...";
			scoreboardEntries[index++].ScoreText.text = score.Value.ToString();
		}
	}
}
