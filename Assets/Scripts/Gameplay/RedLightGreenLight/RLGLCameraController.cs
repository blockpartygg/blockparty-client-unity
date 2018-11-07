using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLCameraController : MonoBehaviour {
	public RLGLPlayer Target;
	public Vector3 Offset;
	public float MovementSpeed;
	public RLGLEliminationManager EliminationManager;
	public RLGLPlayerManager PlayerManager;

	public void SetTarget(RLGLPlayer target) {
		Target = target;
		transform.position = Target.transform.position + Offset;
	}

	void Update() {
		// If the camera isn't tracking an active player, track the next player to be eliminated
		if(Target == null || !Target.Active) {
			if(EliminationManager.PlayerToEliminate != null) {
				if(PlayerManager.Players.ContainsKey(EliminationManager.PlayerToEliminate)) {
					Target = PlayerManager.Players[EliminationManager.PlayerToEliminate];
				}
			}
		}
	}

	void LateUpdate() {
		if(Target != null) {
			transform.position = Vector3.MoveTowards(transform.position, Target.transform.position + Offset, MovementSpeed * Time.deltaTime);
		}
	}
}
