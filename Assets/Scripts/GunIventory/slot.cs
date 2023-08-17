using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class slot : MonoBehaviour
{
    public int slotID;//�ո�ID ���� ��ƷID
    // Start is called before the first frame update
    public Item slotItem;//��Ʒ
    public Image slotImage;//��Ʒͼ��
    public Text slotNum;//��Ʒ������
    public string slotInfo;//��Ʒ��Ϣ
    public GameObject  ItemInSlot;// ȡ����������ʾ��Ϣ�ȹ��ܣ�ֻ����һ������
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
        slotImage.sprite = item.itemImage;//����ƷͼƬ��ʾ��ȥ
        slotNum.text=item.itemHeld.ToString();//��ʾ���ǵĳ�������
        slotInfo = item.itemInfo;//��ʾ���ǵĳ�����Ϣ
        slotdamage = item.thedamage;
        slotdefence = item.thedefence;
    }
}
