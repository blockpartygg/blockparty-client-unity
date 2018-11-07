using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RLGLEliminationRenderer : MonoBehaviour {
	public RLGLEliminationManager EliminationManager;
	public TMP_Text EliminationText;
	
	void Update() {
		if(EliminationManager.PlayerToEliminate != null) {
			if(PlayerManager.Instance.Players.ContainsKey(EliminationManager.PlayerToEliminate)) {
				string playerName = PlayerManager.Instance.Players[EliminationManager.PlayerToEliminate].name;
				string eliminationCountdown = EliminationManager.EliminationCountdown.Seconds.ToString();
				EliminationText.text = playerName + "\nwill be eliminated in " + eliminationCountdown + "...";
			}
		}
	}
}
