using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockKiller : MonoBehaviour {
	public GameObject Root;

	public void Kill() {
		Root.transform.DOScale(Vector3.zero, 1f).OnComplete(() => {
			Destroy(gameObject);
		});
	}

	void Update() {
		if(Input.GetMouseButtonDown(0)) {
			Kill();
		}
	}
}
