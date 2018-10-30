using UnityEngine;
using TMPro;

public class HomeController : MonoBehaviour {
	void Awake() {
		GameManager.Instance.Initialize();
		PlayerManager.Instance.Initialize();
		ChatManager.Instance.Initialize();
	}

	public void SignOut() {
		AuthenticationManager.Instance.SignOut(() => {
			SceneNavigator.Instance.Navigate("TitleScene");
		});
	}

	public void Play() {
		if(AuthenticationManager.Instance.CurrentUser != null) {
			PlayerManager.Instance.SetPlayerPlaying(AuthenticationManager.Instance.CurrentUser.UserId, true);
		}
		
		AnalyticsManager.Instance.LogGameStarted();

		SceneNavigator.Instance.StartPlaying();
	}
}
