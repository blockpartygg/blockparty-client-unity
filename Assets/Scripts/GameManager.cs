using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class GameManager : Singleton<GameManager> {
	public enum GameState {
		PregameCountdown,
		PregameTitle,
		RoundIntroduction,
		MinigameStart,
		MinigamePlay,
		MinigameEnd,
		RoundResultsScoreboard,
		RoundResultsLeaderboard,
		PostgameCelebration,
		PostgameRewards
	}
	public GameState State;
	public event EventHandler StateChanged;
	public DateTime EndTime;
	public event EventHandler EndTimeChanged;
	public long Round;
	public event EventHandler RoundChanged;
	public Minigame Minigame;
	public event EventHandler MinigameChanged;
	public Mode Mode;
	public event EventHandler ModeChanged;
	public Dictionary<string, List<string>> Teams;
	public event EventHandler TeamsChanged;
	public Dictionary<string, long> Scoreboard;
	public event EventHandler ScoreboardChanged;
	public Dictionary<string, long> Leaderboard;
	public event EventHandler LeaderboardChanged;

	void Awake() {
		Minigame = new Minigame("", "", "");
		Mode = new Mode("", "", "");
		Teams = new Dictionary<string, List<string>>();
		Scoreboard = new Dictionary<string, long>();
		Leaderboard = new Dictionary<string, long>();

		DatabaseManager.Instance.SetGameStateChangedHandler(HandleStateChanged);
		DatabaseManager.Instance.SetGameEndTimeChangedHandler(HandleEndTimeChanged);
		DatabaseManager.Instance.SetGameRoundChangedHandler(HandleRoundChanged);
		DatabaseManager.Instance.SetGameMinigameChangedHandler(HandleMinigameChanged);
		DatabaseManager.Instance.SetGameModeChangedHandler(HandleModeChanged);
		DatabaseManager.Instance.SetGameTeamsChangedHandler(HandleTeamsChanged);
		DatabaseManager.Instance.SetGameScoreboardChangedHandler(HandleScoreboardChanged);
		DatabaseManager.Instance.SetGameLeaderboardChangedHandler(HandleLeaderboardChanged);
	}

	void HandleStateChanged(object sender, ValueChangedEventArgs args) {
		if(args.Snapshot.Value != null) {
			string state = args.Snapshot.Value as string;
			State = (GameState)Enum.Parse(typeof(GameState), state, true);
			if(StateChanged != null) {
				StateChanged(this, null);
			}
		}
	}

	void HandleEndTimeChanged(object sender, ValueChangedEventArgs args) {
		if(args.Snapshot.Value != null) {
			string endTimeString = args.Snapshot.Value as string;
			EndTime = DateTime.Parse(endTimeString);
			if(EndTimeChanged != null) {
				EndTimeChanged(this, null);
			}
		}
	}

	void HandleRoundChanged(object sender, ValueChangedEventArgs args) {
		if(args.Snapshot.Value != null) {
			Round = (long)args.Snapshot.Value;
			if(RoundChanged != null) {
				RoundChanged(this, null);
			}
		}
	}

	void HandleMinigameChanged(object sender, ValueChangedEventArgs args) {
		if(args.Snapshot.Value != null) {
			Dictionary<string, object> minigame = args.Snapshot.Value as Dictionary<string, object>;
			if(minigame.ContainsKey("id")) {
				Minigame.Id = minigame["id"] as string;
			}
			if(minigame.ContainsKey("name")) {
				Minigame.Name = minigame["name"] as string;
			}
			if(minigame.ContainsKey("instructions")) {
				Minigame.Instructions = minigame["instructions"] as string;
			}
			if(MinigameChanged != null) {
				MinigameChanged(this, null);
			}
		}
	}

	void HandleModeChanged(object sender, ValueChangedEventArgs args) {
		if(args.Snapshot.Value != null) {
			Dictionary<string, object> mode = args.Snapshot.Value as Dictionary<string, object>;
			if(mode.ContainsKey("id")) {
				Mode.Id = mode["id"] as string;
			}
			if(mode.ContainsKey("name")) {
				Mode.Name = mode["name"] as string;
			}
			if(mode.ContainsKey("instructions")) {
				Mode.Instructions = mode["instructions"] as string;
			}
			if(ModeChanged != null) {
				ModeChanged(this, null);
			}
		}
	}

	void HandleTeamsChanged(object sender, ValueChangedEventArgs args) {
		if(args.Snapshot.Value != null) {
			Teams.Clear();
			Dictionary<string, object> teams = (Dictionary<string, object>)args.Snapshot.Value;
			foreach(KeyValuePair<string, object> team in teams) {
				Teams.Add(team.Key, new List<string>());
				Dictionary<string, object> members = (Dictionary<string, object>)team.Value;
				foreach(KeyValuePair<string, object> member in members) {
					Teams[team.Key].Add(member.Key);
				}
			}
			if(TeamsChanged != null) {
				TeamsChanged(this, null);	
			}
		}
	}

	void HandleScoreboardChanged(object sender, ValueChangedEventArgs args) {
		if(args.Snapshot.Value != null) {
			Scoreboard.Clear();
			Dictionary<string, object> scoreboard = (Dictionary<string, object>)args.Snapshot.Value;
			foreach(KeyValuePair<string, object> score in scoreboard) {
				Scoreboard.Add(score.Key, (long)score.Value);
			}
			if(ScoreboardChanged != null) {
				ScoreboardChanged(this, null);
			}
		}
	}

	void HandleLeaderboardChanged(object sender, ValueChangedEventArgs args) {
		if(args.Snapshot.Value != null) {
			Leaderboard.Clear();
			Dictionary<string, object> leaderboard = (Dictionary<string, object>)args.Snapshot.Value;
			foreach(KeyValuePair<string, object> score in leaderboard) {
				Leaderboard.Add(score.Key, (long)score.Value);
			}
			if(LeaderboardChanged != null) {
				LeaderboardChanged(this, null);
			}
		}
	}

	public void Initialize() {}
}
