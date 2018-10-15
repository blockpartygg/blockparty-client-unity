using UnityEngine;
using Firebase;

public class FirebaseManager : Singleton<FirebaseManager> {
	public FirebaseApp App;

	void Awake() {
		#if UNITY_EDITOR
            AppOptions options = new AppOptions {
                ApiKey = "AIzaSyAUPS3JHPM3CT7yblYiP7zkKkOP2boJ4Zk",
                AppId = "1:547840170646:ios:150474aee52713ab",
                ProjectId = "blockparty-development"
            };
            App = FirebaseApp.Create(options, "Secondary");
		#else
			App = FirebaseApp.DefaultInstance;
        #endif
	}
}
