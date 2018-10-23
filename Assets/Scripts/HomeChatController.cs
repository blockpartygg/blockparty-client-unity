using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HomeChatController : MonoBehaviour {
	GameObject chatContent;
	TMP_InputField chatInputField;
	public GameObject ChatMessagePrefab;

	void Awake() {
		chatContent = GameObject.Find("Chat Content");
		chatInputField = GameObject.Find("Chat Input Field").GetComponent<TMP_InputField>();
	}

	public void SendChatMessage() {
		if(chatInputField.text != "") {
			ChatManager.Instance.AddMessage(Authentication.Instance.CurrentUser.UserId, chatInputField.text);
			chatInputField.text = "";
		}
	}

	void Update() {
		foreach(KeyValuePair<string, ChatMessage> message in ChatManager.Instance.Messages) {
			if(chatContent.transform.Find(message.Key) == null) {
				GameObject messageObject = Instantiate(ChatMessagePrefab, Vector3.zero, Quaternion.identity);
				messageObject.name = message.Key;
				messageObject.transform.Find("Name Text").GetComponent<TMP_Text>().text = PlayerManager.Instance.Players[message.Value.playerId].name;
				messageObject.transform.Find("Message Text").GetComponent<TMP_Text>().text = message.Value.text;
				messageObject.transform.SetParent(chatContent.transform);
			}
		}
	}
}
