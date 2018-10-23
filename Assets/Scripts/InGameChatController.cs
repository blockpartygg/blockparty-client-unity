using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameChatController : MonoBehaviour {
	GameObject chatContent;
	ScrollRect chatScrollRect;
	Scrollbar chatScrollbar;
	GameObject chatObject;
	TMP_InputField chatInputField;

	public GameObject ChatMessagePrefab;

	void Awake() {
		chatContent = GameObject.Find("Chat Content");
		chatScrollRect = GameObject.Find("Chat Scroll View").GetComponent<ScrollRect>();
		chatScrollbar = GameObject.Find("Chat Scrollbar").GetComponent<Scrollbar>();
		chatObject = GameObject.Find("Chat Background");
		chatInputField = GameObject.Find("Chat Input Field").GetComponent<TMP_InputField>();
	}

	public void SendChatMessage() {
		if(chatInputField.text != "") {
			ChatManager.Instance.AddMessage(Authentication.Instance.CurrentUser.UserId, chatInputField.text);
			chatInputField.text = "";
		}
	}

	public void ToggleChat() {
		chatObject.SetActive(!chatObject.activeSelf);
	}

	void Update() {
		foreach(KeyValuePair<string, ChatMessage> message in ChatManager.Instance.Messages) {
			if(chatContent.transform.Find(message.Key) == null) {
				GameObject messageObject = Instantiate(ChatMessagePrefab, Vector3.zero, Quaternion.identity);
				messageObject.name = message.Key;
				messageObject.transform.Find("Message Text").GetComponent<TMP_Text>().text = PlayerManager.Instance.Players[message.Value.playerId].name + " <color=\"white\"><font=\"IBM Plex Sans Condensed - Medium SDF\">" + message.Value.text + "</font></color>";
				messageObject.transform.SetParent(chatContent.transform);

				Canvas.ForceUpdateCanvases();
				chatScrollRect.verticalNormalizedPosition = 0f;
				chatScrollbar.value = 0f;
			}
		}
	}
}