using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregameIntroductionController : MonoBehaviour {
	public void Back() {
		SceneNavigator.Instance.Navigate("HomeScene");
	}
}
