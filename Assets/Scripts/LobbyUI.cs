using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
//大厅界面
public class LobbyUI : MonoBehaviourPunCallbacks
{
    TypedLobby lobby;//大厅对象
    private Transform contentTf;
    private GameObject roomPrefab;
    void Start()
    {
        transform.Find("content/title/closeBtn").GetComponent<Button>().onClick.AddListener(onCloseBtn);
        transform.Find("content/createBtn").GetComponent<Button>().onClick.AddListener(onCreateRoomBtn);
        transform.Find("content/updateBtn").GetComponent<Button>().onClick.AddListener(onUpdateRoomBtn);
        contentTf = transform.Find("content/Scroll View/Viewport/Content");
        roomPrefab= transform.Find("content/Scroll View/Viewport/item").gameObject;

        lobby = new TypedLobby("fpsLobby", LobbyType.SqlLobby);//大厅名字与类型
        //进入大厅
        PhotonNetwork.JoinLobby(lobby);
    }
    //进入大厅回调
    public override void OnJoinedLobby()
    {
        Debug.Log("进入大厅...");
    }
    //创建房间
    public void onCreateRoomBtn()
    {
        Game.uiManager.ShowUI<CreateRoomUI>("CreateRoomUI");
    }
    //关闭大厅界面
    public void onCloseBtn()
    {
        //断开连接
        PhotonNetwork.Disconnect();
        Game.uiManager.CloseUI(gameObject.name);
        //显示登录界面
        Game.uiManager.ShowUI<LoginUI>("LoginUI");
    }
    //刷新房间列表
    public void onUpdateRoomBtn()
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("刷新中...");
        PhotonNetwork.GetCustomRoomList(lobby, "1");//执行该方法后会触发回调

    }
    //清除房间中存在的物体
    private void ClearRoomList()
    {
        while (contentTf.childCount != 0)
        {
            DestroyImmediate(contentTf.GetChild(0).gameObject);
        }
    }
    //刷新房间后的回调
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Game.uiManager.CloseUI("MaskUI");
        Debug.Log("房间刷新");
        ClearRoomList();
        for(int i = 0; i < roomList.Count; i++)
        {
            GameObject obj = Instantiate(roomPrefab, contentTf);
            obj.SetActive(true);
            string roomName = roomList[i].Name;
            obj.transform.Find("roomName").GetComponent<Text>().text = roomName;
            obj.transform.Find("joinBtn").GetComponent<Button>().onClick.AddListener(delegate ()
            {
                Debug.Log(roomName);
                //加入房间
                Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("加入中...");

                PhotonNetwork.JoinRoom(roomName);//加入房间
            });
        }
    }
    public override void OnJoinedRoom()
    {
        //加入房间回调
        Game.uiManager.CloseAllUI();
        Game.uiManager.ShowUI<RoomUI>("RoomUI");

    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //加入房间失败
        Game.uiManager.CloseUI("MaskUI");
    }
}
