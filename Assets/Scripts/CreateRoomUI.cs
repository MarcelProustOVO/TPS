using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviourPunCallbacks
{
    InputField roomNameInput;
    void Start()
    {
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(onCloseBtn);
        transform.Find("bg/okBtn").GetComponent<Button>().onClick.AddListener(onCreateBtn);
        roomNameInput = transform.Find("bg/InputField").GetComponent<InputField>();

        //随机一个房间名称
        roomNameInput.text = "room_" + Random.Range(1, 9999);
    }
    //创建房间
    public void onCreateBtn()
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("创建中...");
        RoomOptions room = new RoomOptions();
        room.MaxPlayers = 8;//最大玩家数
        PhotonNetwork.CreateRoom(roomNameInput.text, room);//房间名称与参数对象
    }
    public void onCloseBtn()
    {
        Game.uiManager.CloseUI(gameObject.name);
    }
    //创建成功回调
    public override void OnCreatedRoom()
    {
        Debug.Log("创建成功");
        Game.uiManager.CloseAllUI();
        //显示房间ui
        Game.uiManager.ShowUI<RoomUI>("RoomUI");
    }
    //创建失败
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Game.uiManager.CloseUI("MaskUI");
    }
}
