using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//��¼����
public class LoginUI : MonoBehaviour,IConnectionCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("startBtn").GetComponent<Button>().onClick.AddListener(onStartBtn);
        transform.Find("quitBtn").GetComponent<Button>().onClick.AddListener(onQuitBtn);
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);//ע��pun2�¼�
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);//ע��pun2�¼�
    }
    public void onStartBtn()
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("���ӷ�����...");
        PhotonNetwork.ConnectUsingSettings();//����pun2������
    }
    public void onQuitBtn()
    {
        Application.Quit();
    }

    public void OnConnected()
    {

    }
    //���ӳɹ���ִ�еĺ���
    public void OnConnectedToMaster()
    {
        Game.uiManager.CloseAllUI();//�رշ����н���
        //��ʾ��������
        Game.uiManager.ShowUI<LobbyUI>("LobbyUI");
    }
    //�Ͽ�������ִ�еĺ���
    public void OnDisconnected(DisconnectCause cause)
    {
        Game.uiManager.CloseUI("MaskUI");
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {

    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {

    }
}
