using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockChaseBlockKiller : MonoBehaviour {
	public GameObject Root;

	public void Kill() {
		Root.transform.DOScale(Vector3.zero, 1f).OnComplete(() => {
			Destroy(gameObject);
		});
	}

	void OnTriggerEnter(Collider other) {
		Kill();
	}
}
