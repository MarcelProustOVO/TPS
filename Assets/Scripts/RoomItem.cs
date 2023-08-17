using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class RoomItem : MonoBehaviour
{
    public int owerId;//��ұ��
    public bool isReady = false;//�Ƿ�׼��

    void Start()
    {
        if (owerId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            transform.Find("Button").GetComponent<Button>().onClick.AddListener(OnReadyBtn);
        }
        else
        {
            transform.Find("Button").GetComponent<Image>().color = Color.black;
        }

        ChangeReady(isReady);
    }

    public void OnReadyBtn()
    {
        isReady = !isReady;
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        table.Add("IsReady", isReady);
        PhotonNetwork.LocalPlayer.SetCustomProperties(table);//�����Զ������
        ChangeReady(isReady);
    }
    public void ChangeReady(bool isReady)
    {
        transform.Find("Button/Text").GetComponent<Text>().text = isReady == true ? "��׼��" : "δ׼��";
    }
}
