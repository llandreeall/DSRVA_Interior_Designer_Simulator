using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSend : MonoBehaviour
{
    public Camera FirstPersonCamera;
    private string IP;
    private int port;


    IPEndPoint remoteEndPoint;
    UdpClient client;


    public void Start()
    {
        /* TODO 1.1 Set your PC IP */
        IP = "192.168.0.248";
        /* TODO 1.2 Set a port to send messages to. You can use 1098 */
        port = 1098;

        /* Setup UDP connection for sending messages */
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();
        //Debug.Log("a facut start din udpsend");
    }

    /* Send data via UDP */
    private void sendMessage(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);
            //Debug.Log("a dat send in udp sendmessage");
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }

    public void Update()
    {
        /* TODO 3 Send the camera orientation and position to the tracking app */
        //sendMessage("ARCore:" + new Quaternion(0.0f, 0.0f, 0.0f, 1.0f).ToString() + ";" + new Vector3(0.0f, 0.0f, 0.0f).ToString());
        //sendMessage("ARCore:" + FirstPersonCamera.transform.rotation.ToString() + ";" + FirstPersonCamera.transform.position.ToString());

        Quaternion rot = FirstPersonCamera.transform.rotation;
        Vector3 pos = FirstPersonCamera.transform.position;
        sendMessage("ARCore:" + new Quaternion(rot.x, rot.y, rot.z, rot.w).ToString()
                        + ";" + new Vector3(pos.x, pos.y, pos.z).ToString());
        //Debug.Log("S.update -- a trimis din send update mesaj cu " + new Vector3(pos.x, pos.y, pos.z).ToString());
    }
}

