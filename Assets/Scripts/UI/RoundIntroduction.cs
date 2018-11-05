using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundIntroduction : MonoBehaviour {
	void Start () {
		AnalyticsManager.Instance.LogRoundStarted("Round " + GameManager.Instance.Round);
	}
}
