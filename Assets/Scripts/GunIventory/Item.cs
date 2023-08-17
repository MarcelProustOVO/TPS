using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//字如其义，真的在右键，新建里面，多出来了这么一项
[CreateAssetMenu(fileName ="New Item",menuName ="Iventory/New Item")]

//继承自这个类的，都是数据库文件，那么都要用上面的那个来存储，
public class Item : ScriptableObject
{
    public string itemName;//物品名称
    public Sprite itemImage;//物品的图片
    public int itemHeld;//物品的持有数量
    public int thedefence;
    public int thedamage;
    [TextArea]
    public string itemInfo;//物品信息   
}
