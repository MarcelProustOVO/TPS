using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //引用了Canvas Group，它有一个，从鼠标向屏幕发射一条射线的功能，返回第一个碰撞的Gameobject
    public Transform originalParent;
    public Inventory mybag;
    public int currentItemID;
    // 当我们开始拖拽,PointerEventData鼠标点击点时间
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        //因为我们直接拖拽的是item组件，所以我们把这个脚本挂载到item组件上面，未激活的item组件不会触发
        originalParent = transform.parent;
        //我们拖动的是slot预制体的Item组件，根据渲染的顺序，
        currentItemID = originalParent.GetComponent<slot>().slotID;
        //有一个父子级渲染的关系，如果不把我们所拖拽的东西提取出来的话
        //那么我们就看不见我们拖拽的东西了
        transform.SetParent(transform.parent.parent.parent);    
        transform.position = eventData.position;
        //因为我们一直拖拽，我们拖拽的transform也一直在跟随着我们，如果我们不把这个设置false，那么我们就会一直检测到我们所拖拽的东西
        //如此一来我们，就没法和其他的进行检测了
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    // 正在拖拽
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        transform.position = eventData.position;
    }
    //结束拖拽
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
                //itemList存储位置变换
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
                //未激活的item不用变，反正之后也要销毁再重建
                //  List[i]还没有进行调整
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                //如果是未激活状态的话，会报错，
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
                    //两边进行图像互换
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
                //如果是防弹衣
                if (originalParent.GetComponent<slot>().slotdamage == 0) { GameObject.Find("Player(Clone)").GetComponent<PlayerController>().Defence = originalParent.GetComponent<slot>().slotdefence;
                    GameObject.Find("Game/Canvas/BagUI/defence").gameObject.GetComponent<slot>().setupslot(mybag.itemList[currentItemID]); }
                //两边进行图像互换
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
