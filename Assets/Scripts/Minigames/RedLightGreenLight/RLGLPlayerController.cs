using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RLGLPlayerController : MonoBehaviour {
	Vector3 jumpBeginScale = new Vector3(1.2f, 0.75f, 1f);
	Vector3 jumpEndScale = new Vector3(0.9f, 0.9f, 0.9f);
	Tweener positionTweener;
	Tweener scaleTweener;
	bool isMoving;

	void Update() {
		if(Input.GetMouseButtonDown(0)) {
			positionTweener = transform.DOMoveY(transform.position.y - 0.25f, 0.2f);
			scaleTweener = transform.DOScale(jumpBeginScale, 0.2f);
		}
		if(Input.GetMouseButtonUp(0)) {	
			if(!isMoving) {
				isMoving = true;
				positionTweener.Kill(true);
				transform.DOJump(transform.position + Vector3.forward + new Vector3(0, 0.25f, 0), 1f, 1, 0.1f).OnComplete(() => transform.position = new Vector3(transform.position.x, 0, transform.position.z));
				scaleTweener.Kill(true);
				transform.DOScale(jumpEndScale, 0.1f).OnComplete(() => isMoving = false);
			}
		}
	}
}
