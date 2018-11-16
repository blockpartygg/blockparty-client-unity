using UnityEngine;

public class NextRowRenderer : MonoBehaviour {
	public BoardRaiser BoardRaiser;

	void Update() {
		Vector3 raiseTranslation = new Vector3(0, BoardRaiser.Elapsed / BoardRaiser.Duration);
		transform.position = transform.parent.position + raiseTranslation;
	}
}