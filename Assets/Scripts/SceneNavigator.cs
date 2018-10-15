using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour {
	static SceneNavigator instance;
	public static SceneNavigator Instance {
		get {
			if(instance == null) {
				instance = (SceneNavigator)FindObjectOfType(typeof(SceneNavigator));
				if(FindObjectsOfType(typeof(SceneNavigator)).Length > 1) {
					return instance;
				}
				if(instance == null) {
					GameObject singleton = new GameObject();
					instance = singleton.AddComponent<SceneNavigator>();
					singleton.name = "Scene Navigator";
					DontDestroyOnLoad(singleton);
				}
			}
			return instance;
		}
	}

	public void Navigate(string sceneName) {
		StartCoroutine(LoadSceneAsync(sceneName));
	}

	IEnumerator LoadSceneAsync(string sceneName) {
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

		while(!asyncLoad.isDone) {
			yield return null;
		}
	}
}
