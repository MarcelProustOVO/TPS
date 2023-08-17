using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    void Start()
    {
        //初始化用户设置
        PhotonNetwork.ConnectUsingSettings();
    }

    //连接到服务器
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("服务器连接成功");

        //创建或者加入房间 设置最大游戏玩家数
        PhotonNetwork.JoinOrCreateRoom("Room1", new Photon.Realtime.RoomOptions { MaxPlayers = 20 }, default);
    }

    //加入到房间
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("加入到房间：" + PhotonNetwork.CurrentRoom);
    }
}