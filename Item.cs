using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string weaponName;
    public float damage;
    public float health;
    public GameObject itemLeft;
    public GameObject itemRight;
    public Item upgradedItem;
}
