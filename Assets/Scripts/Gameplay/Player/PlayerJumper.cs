using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerJumper : MonoBehaviour {
	public GameObject Root;
	bool isJumping;

	void Update() {		
		if(Input.GetMouseButtonDown(0)) {
			Root.transform.DOScale(new Vector3(1.25f, 0.5f, 1.25f), 0.2f);
		}

		if(Input.GetMouseButtonUp(0)) {
			if(!isJumping) {
				isJumping = true;
				Root.transform.DOScale(Vector3.one, 0.2f);
				transform.DOJump(new Vector3(Root.transform.position.x, 0f, Root.transform.position.z -1f), 1f, 1, 0.2f).OnComplete(() => { isJumping = false; });
			}
		}
	}
}
