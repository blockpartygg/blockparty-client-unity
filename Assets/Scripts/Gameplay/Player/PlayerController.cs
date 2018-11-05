using UnityEngine;

public class PlayerController : MonoBehaviour {
	public Vector2 Direction;
	bool isTouching;
	Vector2 origin;
	Vector2 position;
	
	void Awake() {
		origin = new Vector2(Screen.width / 2, Screen.height / 2);
	}
		
	void Update() {
		if(Input.GetMouseButton(0)) {
			isTouching = true;
			position = Input.mousePosition;
		}
		else {
			isTouching = false;
		}
	}

	void FixedUpdate() {
		if(isTouching) {
			Vector2 offset = position - origin;
			Direction = Vector2.ClampMagnitude(offset, 1f);
		}
		else {
			Direction = Vector2.zero;
		}
	}
}
