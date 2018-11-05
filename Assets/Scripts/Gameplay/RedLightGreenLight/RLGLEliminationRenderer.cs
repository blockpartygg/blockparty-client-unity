using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RLGLEliminationRenderer : MonoBehaviour {
	RLGLEliminationManager eliminationManager;
	TMP_Text eliminationText;

	void Awake() {
		eliminationManager = GameObject.Find("Minigame Manager").GetComponent<RLGLEliminationManager>();
		eliminationText = GetComponent<TMP_Text>();
	}
	
	void Update() {
		if(eliminationManager.PlayerToEliminate != null) {
			PlayerManager playerManager = PlayerManager.Instance;
			if(playerManager.Players.ContainsKey(eliminationManager.PlayerToEliminate)) {
				string playerName = PlayerManager.Instance.Players[eliminationManager.PlayerToEliminate].name;
				string eliminationCountdown = eliminationManager.EliminationCountdown.Seconds.ToString();
				eliminationText.text = playerName + "\nwill be eliminated in " + eliminationCountdown + "...";
			}
		}
	}
}
