using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LabelHighlighter : MonoBehaviour {
	TMP_Text labelText;
	public Color SelectedColor;
	public Color DeselectedColor;

	void Awake() {
		labelText = GetComponent<TMP_Text>();
		labelText.color = DeselectedColor;
	}

	public void HandleSelected() {
		labelText.color = SelectedColor;
	}

	public void HandleDeselected() {
		labelText.color = DeselectedColor;
	}
}
