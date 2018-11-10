using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardRenderer : MonoBehaviour {
	TMP_Text firstPlaceName;
	TMP_Text firstPlaceScore;
	GameObject leaderboardContent;
	public GameObject LeaderboardEntryPrefab;

	void Awake() {
		firstPlaceName = GameObject.Find("First Place Name Text").GetComponent<TMP_Text>();
		firstPlaceScore = GameObject.Find("First Place Score Text").GetComponent<TMP_Text>();
		leaderboardContent = GameObject.Find("Leaderboard Content");

		if(GameManager.Instance.Leaderboard.Count > 0) {
			List<KeyValuePair<string, long>> sortedLeaderboard = new List<KeyValuePair<string, long>>(GameManager.Instance.Leaderboard);
			sortedLeaderboard.Sort(delegate(KeyValuePair<string, long> firstPair, KeyValuePair<string, long> secondPair) {
				return firstPair.Value.CompareTo(secondPair.Value);
			});
			sortedLeaderboard.Reverse();

			KeyValuePair<string, long> firstPlaceEntry = sortedLeaderboard[0];
			firstPlaceName.text = PlayerManager.Instance.Players[firstPlaceEntry.Key].name;
			firstPlaceScore.text = firstPlaceEntry.Value + " <size=24>STAR" + (firstPlaceEntry.Value == 1 ? "" : "S") + "</size>";

			sortedLeaderboard.RemoveAt(0);

			int rank = 2;
			
			foreach(KeyValuePair<string, long> entry in sortedLeaderboard) {
				GameObject entryObject = Instantiate(LeaderboardEntryPrefab, Vector3.zero, Quaternion.identity);
				entryObject.transform.Find("Rank Text").GetComponent<TMP_Text>().text = (rank++).ToString();
				entryObject.transform.Find("Name Text").GetComponent<TMP_Text>().text = PlayerManager.Instance.Players[entry.Key].name;
				entryObject.transform.Find("Score Text").GetComponent<TMP_Text>().text = entry.Value + " <size=24>STAR" + (entry.Value == 1 ? "" : "S") + "</size>";
				entryObject.transform.SetParent(leaderboardContent.transform);
				entryObject.transform.localScale = Vector3.one;
			}
		}
	}
}
