using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregameTitleController : MonoBehaviour {
	public void Back() {
		SceneNavigator.Instance.Navigate("HomeScene");
	}
}
