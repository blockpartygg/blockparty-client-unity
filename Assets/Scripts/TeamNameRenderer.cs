using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamNameRenderer : MonoBehaviour {
	public TMP_Text TeamNameText;

	void Update() {
		if(GameManager.Instance.Mode.Id == "redVsBlue") {
			if(Authentication.Instance.CurrentUser != null) {
				if(GameManager.Instance.Teams["redTeamId"].Contains(Authentication.Instance.CurrentUser.UserId)) {
					string teamName = "RED";
					string teamColor = "red";
					TeamNameText.text = "You're on the <font=\"IBM Plex Sans Condensed - Bold SDF\"><color=" + teamColor + ">" + teamName + "</color></font> Team.";
				}
				else if(GameManager.Instance.Teams["blueTeamId"].Contains(Authentication.Instance.CurrentUser.UserId)) {
					string teamName = "BLUE";
					string teamColor = "blue";
					TeamNameText.text = "You're on the <font=\"IBM Plex Sans Condensed - Bold SDF\"><color=" + teamColor + ">" + teamName + "</color></font> Team.";
				}
				else {
					string teamName = "SPECTATING";
					string teamColor = "white";
					TeamNameText.text = "You're on the <font=\"IBM Plex Sans Condensed - Bold SDF\"><color=" + teamColor + ">" + teamName + "</color></font> Team.";
				}
			}
			else {
				TeamNameText.text = "";
			}
		}
	}
}
