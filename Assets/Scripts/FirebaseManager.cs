using UnityEngine;
using Firebase;

public class FirebaseManager : Singleton<FirebaseManager> {
	public FirebaseApp App;

	void Awake() {
		App = FirebaseApp.DefaultInstance;
	}
}
