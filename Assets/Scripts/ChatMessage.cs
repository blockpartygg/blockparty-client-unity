public class ChatMessage {
    public string playerId;
    public string text;
    public long timestamp;

    public ChatMessage(string playerId, string text, long timestamp) {
        this.playerId = playerId;
        this.text = text;
        this.timestamp = timestamp;
    }
}