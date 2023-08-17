using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MoveBag : MonoBehaviour, IDragHandler
{
    RectTransform currentRect;//这个是我们背包ui的transform
    // Start is called before the first frame update
   // eventData是ui那个eventsystem的事件，就是我们触发我们的拖动事件的时候，就会返回这样一个结构体
   //只有我们的鼠标点击到我们的ui上面的时候，才会生成一个eventData，然后这个evnetData会传到这里来
    public void OnDrag(PointerEventData eventData)
    {
        //这个是偷学的，delta好像是变量的意思吧
        currentRect.anchoredPosition += eventData.delta;//返回的是一格二维向量
    }
    private void Awake()
    {
        currentRect=GetComponent<RectTransform>();  
    }
}
