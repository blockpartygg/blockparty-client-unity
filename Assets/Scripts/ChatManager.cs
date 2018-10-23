using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class ChatManager : Singleton<ChatManager> {
	public Dictionary<string, ChatMessage> Messages;

	void Awake() {
		Messages = new Dictionary<string, ChatMessage>();

		Database.Instance.SetMessagesChangedHandler(HandleMessagesChanged);
	}

	void HandleMessagesChanged(object sender, ChildChangedEventArgs args) {
		Dictionary<string, object> message = args.Snapshot.Value as Dictionary<string, object>;

		string playerId = "";
		if(message.ContainsKey("playerId")) {
			playerId = message["playerId"] as string;
		}

		string text = "";
		if(message.ContainsKey("text")) {
			text = message["text"] as string;
		}

		long timestamp = 0;
		if(message.ContainsKey("timestamp")) {
			timestamp = (long)message["timestamp"];
		}

		Messages.Add(args.Snapshot.Key, new ChatMessage(playerId, text, timestamp));
	}

	public void Initialize() {}

	public void AddMessage(string playerId, string text) {
		Database.Instance.AddChatMessage(playerId, text);
	}
}
