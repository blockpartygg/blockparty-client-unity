using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundInstructionsController : MonoBehaviour {
	public void Back() {
		SceneNavigator.Instance.Navigate("HomeScene");
	}
}
