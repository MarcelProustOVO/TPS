using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class RoomItem : MonoBehaviour
{
    public int owerId;//玩家编号
    public bool isReady = false;//是否准备

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
        PhotonNetwork.LocalPlayer.SetCustomProperties(table);//设置自定义参数
        ChangeReady(isReady);
    }
    public void ChangeReady(bool isReady)
    {
        transform.Find("Button/Text").GetComponent<Text>().text = isReady == true ? "已准备" : "未准备";
    }
}
