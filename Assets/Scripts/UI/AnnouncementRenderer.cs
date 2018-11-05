using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnnouncementRenderer : MonoBehaviour {
	Image announcementImage;

	public Sprite StartImage;
	public Sprite PlayImage;
	public Sprite EndImage;

	void Awake() {
		announcementImage = GetComponent<Image>();

		GameManager.Instance.StateChanged += HandleStateChanged;
	}

	void OnDestroy() {
		GameManager.Instance.StateChanged -= HandleStateChanged;
	}

	void Start() {
		announcementImage.sprite = StartImage;
		announcementImage.SetNativeSize();
		transform.localScale = new Vector3(3f, 3f, 3f);
		transform.DOScale(Vector3.one, 0.25f);
		announcementImage.color = Color.white;
		announcementImage.DOColor(Color.clear, 1.0f).SetDelay(1f);
	}

	void HandleStateChanged(object sender, EventArgs args) {
		switch(GameManager.Instance.State) {
		case GameManager.GameState.MinigameStart:
			announcementImage.sprite = StartImage;
			break;
		case GameManager.GameState.MinigamePlay:
			announcementImage.sprite = PlayImage;
			break;
		case GameManager.GameState.MinigameEnd:
			announcementImage.sprite = EndImage;
			break;
		default:
			return;
		}
		announcementImage.SetNativeSize();
		transform.localScale = new Vector3(3f, 3f, 3f);
		transform.DOScale(Vector3.one, 0.25f);
		announcementImage.color = Color.white;
		announcementImage.DOColor(Color.clear, 1.0f).SetDelay(1f);
	}
}
