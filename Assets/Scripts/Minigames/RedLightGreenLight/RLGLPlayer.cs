using System.Collections.Generic;
using BestHTTP.JSON;

public class RLGLPlayer {
    public int positionX;
    public int positionZ;
    public bool moving;
    Dictionary<string, object> dictionary;

    public RLGLPlayer(int positionX, int positionZ, bool moving) {
        this.positionX = positionX;
        this.positionZ = positionZ;
        this.moving = moving;
        dictionary = new Dictionary<string, object>();
    }

    public string ToJSON() {
        dictionary["positionX"] = positionX;
        dictionary["positionZ"] = positionZ;
        dictionary["moving"] = moving;
        string json = Json.Encode(dictionary);

        return json;
    }
}