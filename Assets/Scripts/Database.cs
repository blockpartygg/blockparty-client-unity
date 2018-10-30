using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;

public class Database : Singleton<Database> {
	FirebaseDatabase database;

	void Awake() {
		#if UNITY_EDITOR
			FirebaseManager.Instance.App.SetEditorDatabaseUrl("https://blockparty-development.firebaseio.com/");
			database = FirebaseDatabase.GetInstance(FirebaseManager.Instance.App);
		#else
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://blockparty-production.firebaseio.com/");
			database = FirebaseDatabase.DefaultInstance;
		#endif
	}

	public void SetGameChangedHandler(EventHandler<ValueChangedEventArgs> handler) {
		database.GetReference("game").ValueChanged += handler;
	}

	public void SetGameStateChangedHandler(EventHandler<ValueChangedEventArgs> handler) {
		database.GetReference("game/state").ValueChanged += handler;
	}

	public void SetGameEndTimeChangedHandler(EventHandler<ValueChangedEventArgs> handler) {
		database.GetReference("game/endTime").ValueChanged += handler;
	}

	public void SetGameRoundChangedHandler(EventHandler<ValueChangedEventArgs> handler) {
		database.GetReference("game/round").ValueChanged += handler;
	}

	public void SetGameMinigameChangedHandler(EventHandler<ValueChangedEventArgs> handler) {
		database.GetReference("game/minigame").ValueChanged += handler;
	}

	public void SetGameModeChangedHandler(EventHandler<ValueChangedEventArgs> handler) {
		database.GetReference("game/mode").ValueChanged += handler;
	}

	public void SetGameTeamsChangedHandler(EventHandler<ValueChangedEventArgs> handler) {
		database.GetReference("game/teams").ValueChanged += handler;
	}

	public void SetGameScoreboardChangedHandler(EventHandler<ValueChangedEventArgs> handler) {
		database.GetReference("game/scoreboard").ValueChanged += handler;
	}

	public void SetGameLeaderboardChangedHandler(EventHandler<ValueChangedEventArgs> handler) {
		database.GetReference("game/leaderboard").ValueChanged += handler;
	}

	public void SetPlayersChangedHandler(EventHandler<ValueChangedEventArgs> handler) {
		database.GetReference("players").ValueChanged += handler;
	}
	
	public void SetPlayerValue(string playerId, Player player) {
		string json = JsonUtility.ToJson(player);
		database.GetReference("players/" + playerId).SetRawJsonValueAsync(json);
	}

	public void SetMessagesChangedHandler(EventHandler<ChildChangedEventArgs> handler) {
		database.GetReference("chatMessages").OrderByChild("timestamp").LimitToLast(50).ChildAdded += handler;
	}

	public void AddChatMessage(string playerId, string text) {
		ChatMessage message = new ChatMessage(playerId, text, 0);
		string json = JsonUtility.ToJson(message);
		DatabaseReference reference = database.GetReference("chatMessages").Push();
		reference.SetRawJsonValueAsync(json);
		reference.Child("timestamp").SetValueAsync(ServerValue.Timestamp);
	}
}
