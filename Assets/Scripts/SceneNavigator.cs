using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour {
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
