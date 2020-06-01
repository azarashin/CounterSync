using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class SyncReceiver : SyncTimer
{
    int remotePort = 2002;
    System.Net.Sockets.UdpClient udp; 

    Thread thread;

    private bool _alive = true;

    float _time = 0.0f; 

    private long _localTime; 

    // Start is called before the first frame update
    void Start()
    {
        _alive = true; 
        _localTime = DateTime.Now.Ticks; 
        thread = new Thread(new ThreadStart(Task));
        thread.Start(); 
    }

    // Update is called once per frame
    void Update()
    {
        long current_tick = DateTime.Now.Ticks; 
        float delta = ((current_tick - _localTime) / 10000000.0f); 
        _time = _time + delta; 
        _localTime = current_tick;  
    }

    void OnApplicationQuit()
    {
        _alive = false; 
        thread.Abort();
    }


    private void Task()
    {
        var remote = new IPEndPoint(IPAddress.Any, remotePort);
        var client = new UdpClient(remotePort);
        while(_alive)
        {
         
            var buffer = client.Receive(ref remote);

            _time = BitConverter.ToSingle(buffer, 0); 
            _localTime = DateTime.Now.Ticks; 
        }
        client.Close(); 
    }

 
    public override float CurrentTime()
    {
        return _time;
    }

}
