using UnityEngine;
using System.Collections.Generic;
using BestHTTP.JSON;

public class BlockChasePlayerState : MonoBehaviour {
    public bool Active;
    public Vector3 Position;
    public Vector3 Velocity;

    Dictionary<string, object> dictionary, positionDictionary, velocityDictionary;

    void Awake() {
        dictionary = new Dictionary<string, object>();
        positionDictionary = new Dictionary<string, object>();
        velocityDictionary = new Dictionary<string, object>();
    }

    public string ToJSON() {
        dictionary["active"] = Active;
        positionDictionary["x"] = Position.x;
        positionDictionary["z"] = Position.z;
        dictionary["position"] = positionDictionary;
        velocityDictionary["x"] = Velocity.x;
        velocityDictionary["z"] = Velocity.z;
        dictionary["velocity"] = velocityDictionary;

        string json = Json.Encode(dictionary);

        return json;
    }
}