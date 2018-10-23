using UnityEngine;
using TMPro;

public class HomeController : MonoBehaviour {
	void Awake() {
		GameManager.Instance.Initialize();
		PlayerManager.Instance.Initialize();
		ChatManager.Instance.Initialize();
	}

	public void SignOut() {
		Authentication.Instance.SignOut(() => {
			SceneNavigator.Instance.Navigate("TitleScene");
		});
	}

	public void Play() {
		SceneNavigator.Instance.StartPlaying();
	}
}
