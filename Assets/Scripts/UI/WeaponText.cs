using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponText : MonoBehaviour
{
    public TextMeshProUGUI weaponText; 
    public Player player;

    public void updateGun()
    {
        weaponText.text = player.weapon;
    }
}
