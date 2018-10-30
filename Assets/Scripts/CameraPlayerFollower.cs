using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerFollower : MonoBehaviour {
	public GameObject Player;
	public Vector3 Offset;
	public float MovementSpeed;
	void Start() {
		
	}
	
	void LateUpdate() {
		transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + Offset, MovementSpeed * Time.deltaTime);
	}
}
