using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameChatController : MonoBehaviour {
	public GameObject Content;
	public ScrollRect ScrollRect;
	public Scrollbar Scrollbar;
	public GameObject Object;
	public TMP_InputField InputField;

	public GameObject ChatMessagePrefab;

	public void SendChatMessage() {
		if(InputField.text != "") {
			ChatManager.Instance.AddMessage(AuthenticationManager.Instance.CurrentUser.UserId, InputField.text);
			InputField.text = "";
			AnalyticsManager.Instance.LogChatMessageSent();
		}
	}

	public void ToggleChat() {
		ChatManager.Instance.IsShowing = !ChatManager.Instance.IsShowing;
	}

	void Update() {
		foreach(KeyValuePair<string, ChatMessage> message in ChatManager.Instance.Messages) {
			if(Content.transform.Find(message.Key) == null) {
				if(PlayerManager.Instance.Players.ContainsKey(message.Value.playerId)) {
					GameObject messageObject = Instantiate(ChatMessagePrefab, Vector3.zero, Quaternion.identity);
					messageObject.name = message.Key;
					messageObject.transform.Find("Message Text").GetComponent<TMP_Text>().text = PlayerManager.Instance.Players[message.Value.playerId].name + " <color=\"white\"><font=\"IBM Plex Sans Condensed - Medium SDF\">" + message.Value.text + "</font></color>";
					messageObject.transform.SetParent(Content.transform, false);

					Canvas.ForceUpdateCanvases();
					ScrollRect.verticalNormalizedPosition = 0f;
					Scrollbar.value = 0f;
				}
			}
		}

		if(Object != null) {
			Object.SetActive(ChatManager.Instance.IsShowing);
		}
	}
}