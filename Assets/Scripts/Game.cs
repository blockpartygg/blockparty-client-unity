using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class Game : Singleton<Game> {
	public enum GameState {
		PregameCountdown,
		PregameTitle,
		PregameIntroduction,
		RoundIntroduction,
		RoundInstructions,
		MinigameStart,
		MinigamePlay,
		MinigameEnd,
		RoundResultsScoreboard,
		RoundResultsLeaderboard,
		PostgameCelebration,
		PostgameRewards
	}
	public GameState State;
	public DateTime EndTime;
	public long Round;
	public Minigame Minigame;
	public Mode Mode;

	void Awake() {
		Database.Instance.SetGameChangedHandler(HandleGameChanged);
		Minigame = new Minigame("", "", "");
		Mode = new Mode("", "", "");
	}

	void HandleGameChanged(object sender, ValueChangedEventArgs args) {
		Dictionary<string, object> value = args.Snapshot.Value as Dictionary<string, object>;
		
		if(value != null) {
			if(value.ContainsKey("state")) {
				string state = value["state"] as string;
				State = (GameState)Enum.Parse(typeof(GameState), state, true);
			}

			if(value.ContainsKey("endTime")) {
				string endTimeString = value["endTime"] as string;
				EndTime = DateTime.Parse(endTimeString);
			}

			if(value.ContainsKey("round")) {
				Round = (long)value["round"];
			}

			if(value.ContainsKey("minigame")) {
				Dictionary<string, object> minigame = value["minigame"] as Dictionary<string, object>;
				Minigame.Id = minigame["id"] as string;
				Minigame.Name = minigame["name"] as string;
				Minigame.Instructions = minigame["instructions"] as string;
			}
			
			if(value.ContainsKey("mode")) {
				Dictionary<string, object> mode = value["mode"] as Dictionary<string, object>;
				Mode.Id = mode["id"] as string;
				Mode.Name = mode["name"] as string;
				Mode.Instructions = mode["instructions"] as string;
			}
		}
	}

	public void Initialize() {}
}
