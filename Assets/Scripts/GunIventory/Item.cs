using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�������壬������Ҽ����½����棬���������ôһ��
[CreateAssetMenu(fileName ="New Item",menuName ="Iventory/New Item")]

//�̳��������ģ��������ݿ��ļ�����ô��Ҫ��������Ǹ����洢��
public class Item : ScriptableObject
{
    public string itemName;//��Ʒ����
    public Sprite itemImage;//��Ʒ��ͼƬ
    public int itemHeld;//��Ʒ�ĳ�������
    public int thedefence;
    public int thedamage;
    [TextArea]
    public string itemInfo;//��Ʒ��Ϣ   
}
