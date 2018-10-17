using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregameCountdownController : MonoBehaviour {
	public void Back() {
		SceneNavigator.Instance.Navigate("HomeScene");
	}
}
