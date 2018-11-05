using UnityEngine;
using System;

public class SocketManager : Singleton<SocketManager> {
    BestHTTP.SocketIO.SocketManager manager;
    public BestHTTP.SocketIO.Socket Socket {
        get {
            return manager.Socket;
        }
    }

    public bool IsConnected;
    public event EventHandler Connected;

    public void Initialize() {} // Empty function to wake up the object

    void Awake() {
        #if UNITY_EDITOR
            Connect();
        #endif
    }

    public void Connect() {
        if(!IsConnected) {
            string uri = "";
            #if UNITY_EDITOR
                // uri = "http://192.168.86.51:1337/socket.io/";
                uri = "http://localhost:1337/socket.io/";
            #else
                uri = "http://blockparty-server.herokuapp.com/socket.io/";
            #endif
            BestHTTP.SocketIO.SocketOptions options = new BestHTTP.SocketIO.SocketOptions();
            options.ConnectWith = BestHTTP.SocketIO.Transports.TransportTypes.WebSocket;
            manager = new BestHTTP.SocketIO.SocketManager(new Uri(uri), options);
            manager.Socket.On(BestHTTP.SocketIO.SocketIOEventTypes.Connect, OnConnected);
        }
    }

    void OnConnected(BestHTTP.SocketIO.Socket socket, BestHTTP.SocketIO.Packet packet, object[] args) {
        IsConnected = true;
        if(Connected != null) {
            Connected(this, null);
        }
    }
}