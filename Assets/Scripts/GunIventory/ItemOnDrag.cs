using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //������Canvas Group������һ�������������Ļ����һ�����ߵĹ��ܣ����ص�һ����ײ��Gameobject
    public Transform originalParent;
    public Inventory mybag;
    public int currentItemID;
    // �����ǿ�ʼ��ק,PointerEventData�������ʱ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        //��Ϊ����ֱ����ק����item������������ǰ�����ű����ص�item������棬δ�����item������ᴥ��
        originalParent = transform.parent;
        //�����϶�����slotԤ�����Item�����������Ⱦ��˳��
        currentItemID = originalParent.GetComponent<slot>().slotID;
        //��һ�����Ӽ���Ⱦ�Ĺ�ϵ�����������������ק�Ķ�����ȡ�����Ļ�
        //��ô���ǾͿ�����������ק�Ķ�����
        transform.SetParent(transform.parent.parent.parent);    
        transform.position = eventData.position;
        //��Ϊ����һֱ��ק��������ק��transformҲһֱ�ڸ��������ǣ�������ǲ����������false����ô���Ǿͻ�һֱ��⵽��������ק�Ķ���
        //���һ�����ǣ���û���������Ľ��м����
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    // ������ק
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        transform.position = eventData.position;
    }
    //������ק
    // Start is called before the first frame update
    public void OnEndDrag(PointerEventData eventData)
    {
        if (originalParent.name == "weapon")
        {
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }
        else if(originalParent.name == "defence")
        {
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }
        if (eventData.pointerCurrentRaycast.gameObject == null)
        {
           
         
        }
        else
        {
            if (eventData.pointerCurrentRaycast.gameObject.name == "ItemImage")
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;
                //itemList�洢λ�ñ任
                var temp = mybag.itemList[currentItemID];
                mybag.itemList[currentItemID] = mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID];
                mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID] = temp;

                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "slot(Clone)")
            {
                //δ�����item���ñ䣬����֮��ҲҪ�������ؽ�
                //  List[i]��û�н��е���
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                //�����δ����״̬�Ļ����ᱨ��
                mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID] = mybag.itemList[currentItemID];
                if (eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID == currentItemID) { }
                else { mybag.itemList[currentItemID] = null; }
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "Grid")
            {
                transform.SetParent(originalParent);
                transform.position = originalParent.position;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "BagUI")
            {
                transform.SetParent(originalParent);
                transform.position = originalParent.position;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "WeaponImage")
            {
                if (originalParent.GetComponent<slot>().slotdamage == 0)
                {
                }
                else
                {
                    //���߽���ͼ�񻥻�
                    GameObject.Find("Player(Clone)").GetComponent<PlayerController>().Damage = originalParent.GetComponent<slot>().slotdamage; 
                    GameObject.Find("Game/Canvas/BagUI/weapon").gameObject.GetComponent<slot>().setupslot(mybag.itemList[currentItemID]);
                }
                transform.SetParent(originalParent);
                transform.position = originalParent.position;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "DefenceImage")
            {
                //����Ƿ�����
                if (originalParent.GetComponent<slot>().slotdamage == 0) { GameObject.Find("Player(Clone)").GetComponent<PlayerController>().Defence = originalParent.GetComponent<slot>().slotdefence;
                    GameObject.Find("Game/Canvas/BagUI/defence").gameObject.GetComponent<slot>().setupslot(mybag.itemList[currentItemID]); }
                //���߽���ͼ�񻥻�
                transform.SetParent(originalParent);
                transform.position = originalParent.position;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            else
            {
                transform.SetParent(originalParent);
                transform.position = originalParent.position;

                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
        }
       
    }
}
