public class Player {
    public string name;
    public long currency;
    public long currentSkin;
    // bool playing;

    public Player(string name, long currency, long currentSkin, bool playing) {
        this.name = name;
        this.currency = currency;
        this.currentSkin = currentSkin;
        // this.playing = playing;
    }
}