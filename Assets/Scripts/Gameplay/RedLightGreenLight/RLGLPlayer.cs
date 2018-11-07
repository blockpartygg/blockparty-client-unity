using System.Collections.Generic;
using UnityEngine;
using BestHTTP.JSON;

public class RLGLPlayer : MonoBehaviour {
    public bool Active;
    public Vector3 Position;
    public bool Moving;
    Dictionary<string, object> dictionary;

    void Awake() {
        dictionary = new Dictionary<string, object>();
    }

    public string ToJSON() {
        dictionary["active"] = Active;
        dictionary["positionX"] = Position.x;
        dictionary["positionZ"] = Position.z;
        dictionary["moving"] = Moving;
        
        string json = Json.Encode(dictionary);
        return json;
    }
}