using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Game : MonoBehaviour
{
    public static UIManager uiManager;

    public static bool isLoaded = false;
    private void Awake()
    {
        if (isLoaded)
        {
            Destroy(gameObject);
        }
        else
        {
            isLoaded = true;
            DontDestroyOnLoad(gameObject);//跳转场景当前游戏物体不删除
            uiManager = new UIManager();
            uiManager.Init();

            //设置发送 接收频率 降低延迟
            PhotonNetwork.SendRate = 50;
            PhotonNetwork.SerializationRate = 50;
        }
    }
    void Start()
    {
        //显示登录界面
        uiManager.ShowUI<LoginUI>("LoginUI");
    }

    void Update()
    {
        
    }
}
