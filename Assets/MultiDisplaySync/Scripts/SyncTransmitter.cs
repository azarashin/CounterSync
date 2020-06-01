using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SyncTransmitter : SyncTimer
{
    int remotePort = 2002;
    System.Net.Sockets.UdpClient udp; 

    float _nextTimeToSend = 0.0f; 
    float _interval = 0.5f; 

    // Start is called before the first frame update
    void Start()
    {
        //UdpClientオブジェクトを作成する
        udp = new System.Net.Sockets.UdpClient();
        udp.EnableBroadcast = true;
        udp.Connect(new IPEndPoint(IPAddress.Broadcast, remotePort));
    }

    void OnDestroy()
    {
        udp.Close(); 
    }

    // Update is called once per frame
    void Update()
    {
        _nextTimeToSend -= Time.deltaTime; 
        if(_nextTimeToSend < 0.0f) {
            _nextTimeToSend += _interval; 
            byte[] byteArray = BitConverter.GetBytes(Time.time);
            udp.Send(byteArray, byteArray.Length);
            Debug.Log(string.Format("send: {0}", Time.time));
        }
    }


    override public float CurrentTime()
    {
        return Time.time; 
    }

}
