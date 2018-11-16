using UnityEngine;

public class BlockKiller : MonoBehaviour {
	bool dying;
	float delayElapsed;
	float delayDuration;
	float velocity = 0;
	const float gravityAcceleration = -0.005f;

	public void Kill() {
		dying = true;
		delayDuration = Random.Range(0, 1f);
	}

	void FixedUpdate() {
		if(dying) {
			delayElapsed += Time.deltaTime;

			if(delayElapsed >= delayDuration) {
				velocity += gravityAcceleration;
				transform.Translate(new Vector3(0, velocity));
			}
		}
	}
}