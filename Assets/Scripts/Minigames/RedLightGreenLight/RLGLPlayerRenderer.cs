﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class RLGLPlayerRenderer : MonoBehaviour {
	RLGLPlayer player;
	TMP_Text nameText;
	Vector3 position;
	bool isMoving;
	Tweener positionTweener;
	Tweener scaleTweener;

	void Awake() {
		nameText = transform.Find("Name").GetComponent<TMP_Text>();
		position = new Vector3();
	}

	public void SetPlayer(string playerId, RLGLPlayer player) {
		this.player = player;
		if(PlayerManager.Instance.Players.ContainsKey(playerId)) {
			nameText.text = PlayerManager.Instance.Players[playerId].name;
		}
	}

	void Update() {
		if(position.z != player.positionZ && !isMoving) {
			isMoving = true;
			position.x = player.positionX;
			position.z = player.positionZ;
			transform.DOJump(position, 1f, 1, 0.1f).OnComplete(() => isMoving = false);
		}
	}
}
