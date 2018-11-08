using UnityEngine;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public enum PanelType {
    Combo,
    Chain
}

public class Panel : MonoBehaviour {
	public int Column, Row;
	public SpriteRenderer SpriteRenderer;
	public List<Sprite> Sprites;
	public TMP_Text Text;

	void Start() {
		SpriteRenderer.enabled = false;
		Text.enabled = false;
	}

	public void Play(PanelType type, int value) {
		SpriteRenderer.enabled = true;
		SpriteRenderer.sprite = Sprites[(int)type];
		Text.enabled = true;
		Text.text = type == PanelType.Combo ? value.ToString() : "<size=6>x</size>" + value.ToString();
		transform.position = transform.parent.position + new Vector3(Column, Row, 0f);
		transform.DOMoveY(transform.position.y + 0.5f, 1f).OnComplete(() => { SpriteRenderer.enabled = false; Text.enabled = false; });
	}
}