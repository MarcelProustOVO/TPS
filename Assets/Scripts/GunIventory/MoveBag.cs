using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MoveBag : MonoBehaviour, IDragHandler
{
    RectTransform currentRect;//��������Ǳ���ui��transform
    // Start is called before the first frame update
   // eventData��ui�Ǹ�eventsystem���¼����������Ǵ������ǵ��϶��¼���ʱ�򣬾ͻ᷵������һ���ṹ��
   //ֻ�����ǵ�����������ǵ�ui�����ʱ�򣬲Ż�����һ��eventData��Ȼ�����evnetData�ᴫ��������
    public void OnDrag(PointerEventData eventData)
    {
        //�����͵ѧ�ģ�delta�����Ǳ�������˼��
        currentRect.anchoredPosition += eventData.delta;//���ص���һ���ά����
    }
    private void Awake()
    {
        currentRect=GetComponent<RectTransform>();  
    }
}
