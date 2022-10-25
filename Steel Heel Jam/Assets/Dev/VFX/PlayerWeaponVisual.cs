using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponVisual : MonoBehaviour
{
    public ItemType currentWeapon = ItemType.Unarmed;

    public void ShowWeapon(ItemType item)
    {
        HideWeapon();

        currentWeapon = item;
        if (currentWeapon != ItemType.Unarmed)
        {
            transform.GetChild((int)currentWeapon).gameObject.SetActive(true);
        }
    }

    public void HideWeapon()
    {
        if (currentWeapon != ItemType.Unarmed)
        {
            transform.GetChild((int)currentWeapon).gameObject.SetActive(false);
        }
    }
}
