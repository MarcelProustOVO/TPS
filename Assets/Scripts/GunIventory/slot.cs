using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class slot : MonoBehaviour
{
    public int slotID;//空格ID 等于 物品ID
    // Start is called before the first frame update
    public Item slotItem;//物品
    public Image slotImage;//物品图像
    public Text slotNum;//物品的数量
    public string slotInfo;//物品信息
    public GameObject  ItemInSlot;// 取消掉单击显示信息等功能，只保留一个背景
    public int slotdamage;
    public int slotdefence;
    public void ItemOnclicked()
    {
        InventoryManager.updateItemInfo(slotInfo);
    }
    public void setupslot(Item item)
    {
        if (item == null)
        {
            ItemInSlot.SetActive(false);
            return;
        }
        slotImage.sprite = item.itemImage;//把物品图片显示上去
        slotNum.text=item.itemHeld.ToString();//显示我们的持有数量
        slotInfo = item.itemInfo;//显示我们的持有信息
        slotdamage = item.thedamage;
        slotdefence = item.thedefence;
    }
}
