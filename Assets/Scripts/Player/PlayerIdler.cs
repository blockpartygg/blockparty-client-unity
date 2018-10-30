using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerIdler : MonoBehaviour {
	public GameObject Root;
	
	void Start() {
		Sequence idleSequence = DOTween.Sequence();
		idleSequence.SetEase(Ease.InOutSine);
		idleSequence.Append(Root.transform.DOScale(new Vector3(1.05f, 0.95f, 1.01f), 1f));
		idleSequence.Append(Root.transform.DOScale(Vector3.one, 1f));
		idleSequence.SetLoops(-1);
		idleSequence.Play();
	}
}
