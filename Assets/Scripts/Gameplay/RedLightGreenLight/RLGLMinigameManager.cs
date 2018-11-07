using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLMinigameManager : MonoBehaviour {
	void Awake() {
		AuthenticationManager.Instance.Initialize();
		GameManager.Instance.Initialize();
		SocketManager.Instance.Initialize();
	}
}
