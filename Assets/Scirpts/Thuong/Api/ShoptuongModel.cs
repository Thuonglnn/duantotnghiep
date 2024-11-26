using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoptuongModel
{
    public ShoptuongModel(string username,string generalId, int score, int diamond)
    {
       this.username = username;
        this.generalId = generalId;
        this.score = score;
        this.diamond = diamond;
         

    }

    public string username { get; set; }
    public string generalId { get; set; }
    public int score { get; set; }
    public int diamond { get; set; }

}
