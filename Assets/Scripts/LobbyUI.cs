using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
//��������
public class LobbyUI : MonoBehaviourPunCallbacks
{
    TypedLobby lobby;//��������
    private Transform contentTf;
    private GameObject roomPrefab;
    void Start()
    {
        transform.Find("content/title/closeBtn").GetComponent<Button>().onClick.AddListener(onCloseBtn);
        transform.Find("content/createBtn").GetComponent<Button>().onClick.AddListener(onCreateRoomBtn);
        transform.Find("content/updateBtn").GetComponent<Button>().onClick.AddListener(onUpdateRoomBtn);
        contentTf = transform.Find("content/Scroll View/Viewport/Content");
        roomPrefab= transform.Find("content/Scroll View/Viewport/item").gameObject;

        lobby = new TypedLobby("fpsLobby", LobbyType.SqlLobby);//��������������
        //�������
        PhotonNetwork.JoinLobby(lobby);
    }
    //��������ص�
    public override void OnJoinedLobby()
    {
        Debug.Log("�������...");
    }
    //��������
    public void onCreateRoomBtn()
    {
        Game.uiManager.ShowUI<CreateRoomUI>("CreateRoomUI");
    }
    //�رմ�������
    public void onCloseBtn()
    {
        //�Ͽ�����
        PhotonNetwork.Disconnect();
        Game.uiManager.CloseUI(gameObject.name);
        //��ʾ��¼����
        Game.uiManager.ShowUI<LoginUI>("LoginUI");
    }
    //ˢ�·����б�
    public void onUpdateRoomBtn()
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("ˢ����...");
        PhotonNetwork.GetCustomRoomList(lobby, "1");//ִ�и÷�����ᴥ���ص�

    }
    //��������д��ڵ�����
    private void ClearRoomList()
    {
        while (contentTf.childCount != 0)
        {
            DestroyImmediate(contentTf.GetChild(0).gameObject);
        }
    }
    //ˢ�·����Ļص�
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Game.uiManager.CloseUI("MaskUI");
        Debug.Log("����ˢ��");
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
                //���뷿��
                Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("������...");

                PhotonNetwork.JoinRoom(roomName);//���뷿��
            });
        }
    }
    public override void OnJoinedRoom()
    {
        //���뷿��ص�
        Game.uiManager.CloseAllUI();
        Game.uiManager.ShowUI<RoomUI>("RoomUI");

    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //���뷿��ʧ��
        Game.uiManager.CloseUI("MaskUI");
    }
}
