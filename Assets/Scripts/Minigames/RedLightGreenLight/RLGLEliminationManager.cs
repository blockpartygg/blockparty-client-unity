using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLEliminationManager : MonoBehaviour {
	public TimeSpan EliminationCountdown;
	public string PlayerToEliminate;

	public void SetEliminationCountdown(double timeRemaining) {
		EliminationCountdown = TimeSpan.FromMilliseconds(timeRemaining);
	}
}
