using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarRenderer : MonoBehaviour {
    Player player;
    public List<GameObject> AvatarPrefabs;
    public GameObject AvatarMesh;

    public void SetPlayer(string playerId) {
        if(PlayerManager.Instance.Players.ContainsKey(playerId)) {
            player = PlayerManager.Instance.Players[playerId];
            GameObject avatar = Instantiate(AvatarPrefabs[(int)player.currentSkin], Vector3.zero, Quaternion.identity);
            avatar.transform.SetParent(AvatarMesh.transform);
        }
    }
}