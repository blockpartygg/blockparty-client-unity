using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundLeaderboard : MonoBehaviour {
	void Start () {
		Analytics.Instance.LogRoundEnded("Round " + GameManager.Instance.Round);
	}
}
