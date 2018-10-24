using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlayerRenderer : MonoBehaviour {
	long previousSkin;
	public List<GameObject> AvatarSkinPrefabs;
	public GameObject AvatarMesh;
	public TMP_Text NameText;

	void Awake() {
		if(Authentication.Instance.CurrentUser != null) {
			if(PlayerManager.Instance.Players.ContainsKey(Authentication.Instance.CurrentUser.UserId)) {
				Player player = PlayerManager.Instance.Players[Authentication.Instance.CurrentUser.UserId];
				previousSkin = player.currentSkin;

				GameObject avatar = Instantiate(AvatarSkinPrefabs[(int)player.currentSkin], Vector3.zero, Quaternion.identity);
				avatar.transform.SetParent(AvatarMesh.transform);

				NameText.text = player.name;
			}
		}
	}

	void Update() {
		if(Authentication.Instance.CurrentUser != null) {
			Player player = PlayerManager.Instance.Players[Authentication.Instance.CurrentUser.UserId];

			if(player.currentSkin != previousSkin) {
				AvatarMesh.transform.DOScale(Vector3.zero, 1.0f).OnComplete(() => {
					foreach(Transform child in AvatarMesh.transform) {
						Destroy(child.gameObject);
					}
					GameObject avatar = Instantiate(AvatarSkinPrefabs[(int)player.currentSkin], Vector3.zero, Quaternion.identity);
					avatar.transform.SetParent(AvatarMesh.transform);
					avatar.transform.localScale = Vector3.one;
					AvatarMesh.transform.DOScale(Vector3.one, 1.0f);
				});
			}

			previousSkin = player.currentSkin;
		}
	}
}
