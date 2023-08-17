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
            DontDestroyOnLoad(gameObject);//��ת������ǰ��Ϸ���岻ɾ��
            uiManager = new UIManager();
            uiManager.Init();

            //���÷��� ����Ƶ�� �����ӳ�
            PhotonNetwork.SendRate = 50;
            PhotonNetwork.SerializationRate = 50;
        }
    }
    void Start()
    {
        //��ʾ��¼����
        uiManager.ShowUI<LoginUI>("LoginUI");
    }

    void Update()
    {
        
    }
}
