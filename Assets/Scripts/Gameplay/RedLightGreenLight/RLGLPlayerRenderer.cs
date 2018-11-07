using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class RLGLPlayerRenderer : MonoBehaviour {
	public RLGLPlayer Player;
	bool isMoving;
	bool isEliminated;

	void Update() {
		if((transform.position.x != Player.Position.x || transform.position.z != Player.Position.z) && !isMoving) {
			isMoving = true;
			Vector3 position = new Vector3(Player.Position.x, 0, Player.Position.z);
			transform.DOJump(position, 1f, 1, 0.1f).OnComplete(() => isMoving = false);
		}

		if(!isEliminated && !Player.Active) {
			isEliminated = true;
			transform.DOScale(Vector3.zero, 3.0f);
			transform.DORotate(new Vector3(0.0f, 1080.0f, 0.0f), 3.0f, RotateMode.FastBeyond360);
		}
	}
}
