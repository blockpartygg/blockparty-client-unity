using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RLGLPlayerController : MonoBehaviour {
	public bool MoveReleased;

	void Update() {
		MoveReleased = Input.GetMouseButtonUp(0);
	}
}
