using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopitemModel
{
    public ShopitemModel(string username,string itemId, int score, int diamond)
    {
       this.username = username;
        this.itemId = itemId;
        this.score = score;
        this.diamond = diamond;
         

    }

    public string username { get; set; }
    public string itemId { get; set; }
    public int score { get; set; }
    public int diamond { get; set; }

}
