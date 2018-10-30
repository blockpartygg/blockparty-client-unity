using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class PlayerManager : Singleton<PlayerManager> {
	public Dictionary<string, Player> Players;

	void Awake() {
		Players = new Dictionary<string, Player>();

		DatabaseManager.Instance.SetPlayersChangedHandler(HandlePlayersChanged);
	}

	void HandlePlayersChanged(object sender, ValueChangedEventArgs args) {
		Dictionary<string, object> players = args.Snapshot.Value as Dictionary<string, object>;

		foreach(KeyValuePair<string, object> player in players) {
			string playerId = player.Key;
			Dictionary<string, object> playerDictionary = player.Value as Dictionary<string, object>;

			string name = "";
			if(playerDictionary.ContainsKey("name")) {
				name = playerDictionary["name"] as string;
			}

			long currency = 0;
			if(playerDictionary.ContainsKey("currency")) {
				currency = (long)playerDictionary["currency"];
			}

			long skin = 0;
			if(playerDictionary.ContainsKey("currentSkin")) {
				skin = (long)playerDictionary["currentSkin"];
			}

			bool playing = false;
			if(playerDictionary.ContainsKey("playing")) {
				playing = (bool)playerDictionary["playing"];
			}

			Players[playerId] = new Player(name, currency, skin, playing);
		}
	}

	public bool PurchaseAndSetSkin(string playerId, long skin, long amount) {
		Player player = PlayerManager.Instance.Players[playerId];

		if(player.currency < amount) {
			return false;
		}

		DatabaseManager.Instance.SetPlayerValue(playerId, new Player(player.name, player.currency - amount, skin, player.playing));

		return true;
	}

	public void SetPlayerSkin(string playerId, long skin) {
		Player player = PlayerManager.Instance.Players[playerId];

		DatabaseManager.Instance.SetPlayerValue(playerId, new Player(player.name, player.currency, skin, player.playing));
	}

	public void SetPlayerPlaying(string playerId, bool playing) {
		Player player = PlayerManager.Instance.Players[playerId];

		DatabaseManager.Instance.SetPlayerValue(playerId, new Player(player.name, player.currency, player.currentSkin, playing));
	}

	public void TransactPlayerCurrency(string playerId, long amount) {
		Player player = PlayerManager.Instance.Players[playerId];

		DatabaseManager.Instance.SetPlayerValue(playerId, new Player(player.name, player.currency + amount, player.currentSkin, true));
	}

	public void Initialize() {}
}
