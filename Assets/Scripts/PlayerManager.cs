using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class PlayerManager : Singleton<PlayerManager> {
	public Dictionary<string, Player> Players;

	void Awake() {
		Players = new Dictionary<string, Player>();

		Database.Instance.SetPlayersChangedHandler(HandlePlayersChanged);
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

			long currentSkin = 0;
			if(playerDictionary.ContainsKey("currentSkin")) {
				currentSkin = (long)playerDictionary["currentSkin"];
			}

			bool playing = false;
			if(playerDictionary.ContainsKey("playing")) {
				playing = (bool)playerDictionary["playing"];
			}

			Players[playerId] = new Player(name, currency, currentSkin, playing);
		}
	}

	public void TransactPlayerCurrency(string playerId, long amount) {
		Player player = PlayerManager.Instance.Players[playerId];

		Database.Instance.SetPlayerValue(playerId, new Player(player.name, player.currency + amount, player.currentSkin, true));
	}

	public void Initialize() {}
}
