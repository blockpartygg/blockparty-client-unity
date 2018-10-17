using UnityEngine;
using System;
using BestHTTP;
using BestHTTP.SocketIO;

public class SocketIO : Singleton<SocketIO> {
    SocketManager manager;
    public Socket Socket {
        get {
            return manager.Socket;
        }
    }

    public void Connect() {
        string uri = "";
        #if UNITY_EDITOR
            uri = "http://localhost:1337/socket.io/";
        #else
            uri = "http://blockparty-server.herokuapp.com/socket.io/";
        #endif
        SocketOptions options = new SocketOptions();
        options.ConnectWith = BestHTTP.SocketIO.Transports.TransportTypes.WebSocket;
        manager = new SocketManager(new Uri(uri), options);
        manager.Socket.On(SocketIOEventTypes.Connect, OnConnected);
    }

    void OnConnected(Socket socket, Packet packet, object[] args) {
        Debug.LogWarning("Socket connected: " + socket.Id);
    }
}