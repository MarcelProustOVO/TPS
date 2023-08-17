using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//登录界面
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
        PhotonNetwork.AddCallbackTarget(this);//注册pun2事件
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);//注销pun2事件
    }
    public void onStartBtn()
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("连接服务器...");
        PhotonNetwork.ConnectUsingSettings();//连接pun2服务器
    }
    public void onQuitBtn()
    {
        Application.Quit();
    }

    public void OnConnected()
    {

    }
    //连接成功后执行的函数
    public void OnConnectedToMaster()
    {
        Game.uiManager.CloseAllUI();//关闭服所有界面
        //显示大厅界面
        Game.uiManager.ShowUI<LobbyUI>("LobbyUI");
    }
    //断开服务器执行的函数
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
