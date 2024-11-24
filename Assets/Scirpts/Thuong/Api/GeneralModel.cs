using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralModel 
{
    public GeneralModel(string username, string generalId)
    {
        this.username = username;
        this.generalId = generalId;
    }

    public string username { get; set; }
   public string generalId { get; set; }
 

}

