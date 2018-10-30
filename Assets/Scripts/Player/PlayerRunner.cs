using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerRunner : MonoBehaviour {
	public GameObject Root;
	public PlayerJoystickController Controller;
	public float MovementSpeed;
	public float RotationSpeed;
	public float StepHeight;
	public float StepDuration;
	bool isStepping;

	void FixedUpdate() {
		if(Controller.Direction != Vector2.zero) {
			Vector3 direction = new Vector3(Controller.Direction.x, 0f, Controller.Direction.y);
			Root.transform.Translate(direction * MovementSpeed * Time.deltaTime, Space.World);
			if(!isStepping) {
				isStepping = true;
				Root.transform.DOMoveY(StepHeight, StepDuration / 2);
				Root.transform.DOMoveY(0f, StepDuration / 2).SetDelay(StepDuration / 2).OnComplete(() => { isStepping = false; });
			}
			Root.transform.rotation = Quaternion.RotateTowards(Root.transform.rotation, Quaternion.LookRotation(direction * -1), RotationSpeed * Time.deltaTime);
		}
	}
}
