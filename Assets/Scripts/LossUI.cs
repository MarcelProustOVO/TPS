using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LossUI : MonoBehaviour
{
    public System.Action onClickCallBack;//委托,类似于函数指针
    void Start()
    {
        transform.Find("resetBtn").GetComponent<Button>().onClick.AddListener(OnClickBtn);
    }

    public void OnClickBtn()
    {
        onClickCallBack?.Invoke();//判断一下这个委托是不是为null；如果是则不执行委托，如果不是则执行该委托；
                                  //invoke：在拥有此空间基础窗口句柄的线程上执行指定的委托

        //begininvoke：在创建控件的基础句柄所在的线程上异步执行指定的委托
        Game.uiManager.CloseUI(gameObject.name);
    }
}
