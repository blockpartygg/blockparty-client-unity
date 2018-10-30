using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundLeaderboard : MonoBehaviour {
	void Start () {
		AnalyticsManager.Instance.LogRoundEnded("Round " + GameManager.Instance.Round);
	}
}
