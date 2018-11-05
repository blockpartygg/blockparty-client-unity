﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class ChatManager : Singleton<ChatManager> {
	public Dictionary<string, ChatMessage> Messages;
	public event EventHandler MessagesChanged;
	public bool IsShowing;

	void Awake() {
		Messages = new Dictionary<string, ChatMessage>();
		IsShowing = true;
		
		DatabaseManager.Instance.SetMessagesChangedHandler(HandleMessagesChanged);
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
		if(MessagesChanged != null) {
			MessagesChanged(this, null);
		}
	}

	public void Initialize() {}

	public void AddMessage(string playerId, string text) {
		DatabaseManager.Instance.AddChatMessage(playerId, text);
	}
}