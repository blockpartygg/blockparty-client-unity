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

	GameManager.GameState previousState;

	void Awake() {
		announcementImage = GetComponent<Image>();
	}

	void Update() {
		if(previousState != GameManager.Instance.State) {
			previousState = GameManager.Instance.State;

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
			}
			announcementImage.SetNativeSize();
			transform.localScale = Vector3.zero;
			transform.DOScale(Vector3.one, 1.0f);
			announcementImage.color = Color.white;
			announcementImage.DOColor(Color.clear, 1.0f).SetDelay(1.0f);
		}
	}
}
