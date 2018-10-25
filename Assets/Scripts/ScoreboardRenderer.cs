using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreboardRenderer : MonoBehaviour {
	TMP_Text firstPlaceName;
	TMP_Text firstPlaceScore;
	GameObject scoreboardContent;
	public GameObject ScoreboardEntryPrefab;

	void Awake() {
		firstPlaceName = GameObject.Find("First Place Name Text").GetComponent<TMP_Text>();
		firstPlaceScore = GameObject.Find("First Place Score Text").GetComponent<TMP_Text>();
		scoreboardContent = GameObject.Find("Scoreboard Content");

		List<KeyValuePair<string, long>> sortedScoreboard = new List<KeyValuePair<string, long>>(GameManager.Instance.Scoreboard);
		sortedScoreboard.Sort(delegate(KeyValuePair<string, long> firstPair, KeyValuePair<string, long> secondPair) {
			return firstPair.Value.CompareTo(secondPair.Value);
		});
		sortedScoreboard.Reverse();

		KeyValuePair<string, long> firstPlaceEntry = sortedScoreboard[0];
		firstPlaceName.text = PlayerManager.Instance.Players[firstPlaceEntry.Key].name;
		firstPlaceScore.text = firstPlaceEntry.Value.ToString() + " <size=24>POINTS</size>";

		sortedScoreboard.RemoveAt(0);

		int rank = 2;
		
		foreach(KeyValuePair<string, long> entry in sortedScoreboard) {
			GameObject entryObject = Instantiate(ScoreboardEntryPrefab, Vector3.zero, Quaternion.identity);
			entryObject.transform.Find("Rank Text").GetComponent<TMP_Text>().text = (rank++).ToString();
			entryObject.transform.Find("Name Text").GetComponent<TMP_Text>().text = PlayerManager.Instance.Players[entry.Key].name;
			entryObject.transform.Find("Score Text").GetComponent<TMP_Text>().text = entry.Value.ToString() + " <size=24>POINTS</size>";
			entryObject.transform.SetParent(scoreboardContent.transform);
			entryObject.transform.localScale = Vector3.one;
		}
	}
}
