using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LossUI : MonoBehaviour
{
    public System.Action onClickCallBack;//ί��,�����ں���ָ��
    void Start()
    {
        transform.Find("resetBtn").GetComponent<Button>().onClick.AddListener(OnClickBtn);
    }

    public void OnClickBtn()
    {
        onClickCallBack?.Invoke();//�ж�һ�����ί���ǲ���Ϊnull���������ִ��ί�У����������ִ�и�ί�У�
                                  //invoke����ӵ�д˿ռ�������ھ�����߳���ִ��ָ����ί��

        //begininvoke���ڴ����ؼ��Ļ���������ڵ��߳����첽ִ��ָ����ί��
        Game.uiManager.CloseUI(gameObject.name);
    }
}
