using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour,IInRoomCallbacks
{
    Transform startTf;
    Transform contentTf;
    GameObject roomPrefab;
    public List<RoomItem> roomList;
    private void Awake()
    {
        roomList = new List<RoomItem>();
        contentTf = transform.Find("bg/Content");
        roomPrefab = transform.Find("bg/roomItem").gameObject;
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(onClodeBtn);
        startTf = transform.Find("bg/startBtn");
        startTf.GetComponent<Button>().onClick.AddListener(onStartBtn);

        PhotonNetwork.AutomaticallySyncScene = true;//执行加载场景的时候 其他玩家也跳转

    }
    void Start()
    {
        //生成房间里的玩家项
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Player p = PhotonNetwork.PlayerList[i];
            CreateRoomItem(p);
        }
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    //生成玩家
    public void CreateRoomItem(Player p)
    {
        GameObject obj = Instantiate(roomPrefab, contentTf);
        obj.SetActive(true);
        RoomItem item = obj.AddComponent<RoomItem>();
        item.owerId = p.ActorNumber;//玩家编号
        roomList.Add(item);

        object val;
        if(p.CustomProperties.TryGetValue("IsReady",out val))
        {
            item.isReady = (bool)val;
        }

        //如果是主机玩家 判断所有玩家准备状态
        if (PhotonNetwork.IsMasterClient)
        {
            bool isAllReady = true;
            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].isReady == false)
                {
                    isAllReady = false;
                    break;
                }
            }
            startTf.gameObject.SetActive(isAllReady);//开始按钮是否显示
        }
    }
    //删除离开房间的玩家
    public void DeleteRoomItem(Player p)
    {
        RoomItem item = roomList.Find((RoomItem _item) => { return p.ActorNumber == _item.owerId; });
        if (item != null)
        {
            Destroy(item.gameObject);
            roomList.Remove(item);
        }
    }
    //关闭
    void onClodeBtn()
    {
        //断开连接
        PhotonNetwork.Disconnect();
        Game.uiManager.CloseUI(gameObject.name);
        Game.uiManager.ShowUI<LoginUI>("LoginUI");
    }
    //开始游戏
    void onStartBtn()
    {
        //加载场景 让房间内的玩家也加载场景
        PhotonNetwork.LoadLevel("game");
    }
    //新玩家进入房间
    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        CreateRoomItem(newPlayer);
    }
    //房间里其他玩家离开房间
    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        DeleteRoomItem(otherPlayer);
    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        
    }
    //玩家自定义参数更新回调
    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        RoomItem item = roomList.Find((_item) => { return _item.owerId == targetPlayer.ActorNumber; });
        if (item != null)
        {
            item.isReady = (bool)changedProps["IsReady"];
            item.ChangeReady(item.isReady);
        }
        //如果是主机玩家 判断所有玩家准备状态
        if (PhotonNetwork.IsMasterClient)
        {
            bool isAllReady = true;
            for(int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].isReady == false)
                {
                    isAllReady = false;
                    break;
                }
            }
            startTf.gameObject.SetActive(isAllReady);//开始按钮是否显示
        }
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
        
    }
}
