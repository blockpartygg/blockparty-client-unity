using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveGameNavigator : MonoBehaviour {
	public void LeaveGame() {
		SceneNavigator.Instance.StopPlaying();
	}
}
