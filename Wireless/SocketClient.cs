﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SocketClient
{
    private Socket socketClient;
    private Thread thread;
    private byte[] data = new byte[1024];

    public bool isTrigger = false;
    public float x, y, z, w;
    
    public SocketClient(string hostIP, int port) {

        thread = new Thread(() => {
            // while the status is "Disconnect", this loop will keep trying to connect.
            while (true) {
                try {
                    socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socketClient.Connect(new IPEndPoint(IPAddress.Parse(hostIP), port));
                    // while the connection
                    while (true) {
                        /*********************************************************
                         * TODO: you need to modify receive function by yourself *
                         *********************************************************/
                        byte[] start = Encoding.UTF8.GetBytes("ok");
                        socketClient.Send(start);

                        // if (socketClient.Available < 100) {
                        //     Debug.Log(socketClient.Available);
                        //     Thread.Sleep(1);
                        //     continue;
                        // }
                        int length = socketClient.Receive(data);
                        string message = Encoding.UTF8.GetString(data, 0, length);
                        Debug.Log("Recieve message: " + message);

                        var parts = message.Split(' ');
                        isTrigger = Convert.ToBoolean(parts[0]);
                        w = float.Parse(parts[1]);
                        x = float.Parse(parts[2]);
                        y = float.Parse(parts[3]);
                        z = float.Parse(parts[4]);
                        // */
                    }
                } catch (Exception ex) {
                    if (socketClient != null) {
                        socketClient.Close();
                    }
                    Debug.Log("exception: " + ex.Message);
                }
            }
        });
        thread.IsBackground = true;
        thread.Start();
    }

    public void Close() {
        thread.Abort();
        if (socketClient != null) {
            socketClient.Close();
        }
    }
}
