using UnityEngine;

public class NextRowRenderer : MonoBehaviour {
	public BoardRaiser BoardRaiser;
	Vector3 initialPosition;

	void Start() {
		initialPosition = transform.position;
	}

	void Update() {
		Vector3 raiseTranslation = initialPosition + new Vector3(0, BoardRaiser.Elapsed / BoardRaiser.Duration);

		transform.position = raiseTranslation;
	}
}