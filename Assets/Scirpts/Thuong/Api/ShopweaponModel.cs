using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopweaponModel
{
    public ShopweaponModel(string username, string weaponId, int score, int diamond)
    {
        this.username = username;
        this.weaponId = weaponId;
        this.score = score;
        this.diamond = diamond;
    }

    public string username { get; set; }
    public string weaponId { get; set; }
    public int score { get; set; }
    public int diamond { get; set; }

}
