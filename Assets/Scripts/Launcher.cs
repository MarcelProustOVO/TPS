using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    void Start()
    {
        //��ʼ���û�����
        PhotonNetwork.ConnectUsingSettings();
    }

    //���ӵ�������
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("���������ӳɹ�");

        //�������߼��뷿�� ���������Ϸ�����
        PhotonNetwork.JoinOrCreateRoom("Room1", new Photon.Realtime.RoomOptions { MaxPlayers = 20 }, default);
    }

    //���뵽����
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("���뵽���䣺" + PhotonNetwork.CurrentRoom);
    }
}