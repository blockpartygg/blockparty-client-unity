using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundIntroduction : MonoBehaviour {
	void Start () {
		Analytics.Instance.LogRoundStarted("Round " + GameManager.Instance.Round);
	}
}
