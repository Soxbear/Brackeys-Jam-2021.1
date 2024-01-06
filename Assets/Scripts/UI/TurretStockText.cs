using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretStockText : MonoBehaviour
{
    public TextMeshProUGUI turretStockText; 
    public Player player;

    public void updateStock()
    {
        turretStockText.text = "" + player.turretStock;
    }
}
