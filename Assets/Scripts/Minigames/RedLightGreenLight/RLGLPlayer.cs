using System.Collections.Generic;
using BestHTTP.JSON;

public class RLGLPlayer {
    public bool active;
    public int positionX;
    public int positionZ;
    public bool moving;
    Dictionary<string, object> dictionary;

    public RLGLPlayer(bool active, int positionX, int positionZ, bool moving) {
        this.active = active;
        this.positionX = positionX;
        this.positionZ = positionZ;
        this.moving = moving;
        dictionary = new Dictionary<string, object>();
    }

    public string ToJSON() {
        dictionary["active"] = active;
        dictionary["positionX"] = positionX;
        dictionary["positionZ"] = positionZ;
        dictionary["moving"] = moving;
        string json = Json.Encode(dictionary);

        return json;
    }
}