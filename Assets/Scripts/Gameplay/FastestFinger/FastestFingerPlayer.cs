using UnityEngine;
using System.Collections.Generic;
using BestHTTP.JSON;

public class FastestFingerPlayer : MonoBehaviour {
    public bool Active;
    public Vector3 Position;
    public bool Moving;
    Dictionary<string, object> stateDictionary, positionDictionary;

    void Awake() {
        stateDictionary = new Dictionary<string, object>();
        positionDictionary = new Dictionary<string, object>();
    }

    public string ToJSON() {
        stateDictionary["active"] = Active;
        positionDictionary["x"] = Position.x;
        positionDictionary["z"] = Position.z;
        stateDictionary["position"] = positionDictionary;
        stateDictionary["moving"] = Moving;

        string json = Json.Encode(stateDictionary);
        return json;
    }
}