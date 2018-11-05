using System.Collections.Generic;
using UnityEngine;
using BestHTTP.JSON;

public class RLGLPlayer : MonoBehaviour {
    public bool active;
    public int positionX;
    public int positionZ;
    public bool moving;
    Dictionary<string, object> dictionary;
    PlayerRenderer playerRenderer;
    public PlayerRenderer PlayerRendererPrefab;

    void Awake() {
        dictionary = new Dictionary<string, object>();

        playerRenderer = Instantiate(PlayerRendererPrefab, Vector3.zero, Quaternion.identity);
        playerRenderer.transform.SetParent(transform);
    }

    void Start() {

    }

    public void Initialize(string playerId, bool active, int positionX, int positionZ, bool moving) {
        this.active = active;
        this.positionX = positionX;
        this.positionZ = positionZ;
        this.moving = moving;
        playerRenderer.SetPlayer(playerId);
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