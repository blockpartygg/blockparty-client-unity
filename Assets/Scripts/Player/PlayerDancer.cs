using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDancer : MonoBehaviour {
	public GameObject Root;

	void Start () {
		
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(1)) {
			Sequence danceSequence = DOTween.Sequence();
			danceSequence.Append(Root.transform.DORotate(new Vector3(0f, 30f, 0f), 0.2f).SetEase(Ease.OutSine));
			danceSequence.Append(Root.transform.DORotate(new Vector3(0f, -360f, 0f), 1f, RotateMode.FastBeyond360));
			danceSequence.Play();
			Root.transform.DOJump(Vector3.zero, 0.5f, 1, 0.4f).SetDelay(0.4f);
		}	
	}
}
