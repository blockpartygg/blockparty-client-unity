using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlayerRenderer : MonoBehaviour {
	string playerId;
	Player player;
	long previousSkin;
	public List<GameObject> AvatarSkinPrefabs;
	public GameObject AvatarMesh;
	public TMP_Text NameText;

	public void SetPlayer(string playerId) {
		if(PlayerManager.Instance.Players.ContainsKey(playerId)) {
			this.playerId = playerId;
			player = PlayerManager.Instance.Players[playerId];
			previousSkin = player.currentSkin;

			GameObject avatar = Instantiate(AvatarSkinPrefabs[(int)player.currentSkin], Vector3.zero, Quaternion.identity);
			avatar.transform.SetParent(AvatarMesh.transform);

			NameText.text = player.name;
		}
	}

	void Update() {
		player = PlayerManager.Instance.Players[playerId];
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
