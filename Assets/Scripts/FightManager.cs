using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FightManager : MonoBehaviour
{
    private void Awake()
    {
        //�������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //�ر����н���
        Game.uiManager.CloseAllUI();
        //��ʾս������
        Game.uiManager.ShowUI<FightUI>("FightUI");
        Game.uiManager.ShowUI<MoveBag>("BagUI");
        Transform pointTf = GameObject.Find("Point").transform;
        Vector3 pos = pointTf.GetChild(Random.Range(0, pointTf.childCount)).position;
        //ʵ������ɫ
        PhotonNetwork.Instantiate("Player", pos, Quaternion.identity);//ʵ��������Դ������Դ�ļ���

    }
}
