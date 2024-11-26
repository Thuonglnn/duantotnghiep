using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondModel 
{
    public DiamondModel(string username, int diamond)
    {
        this.username = username;
        this.diamond = diamond;
    }

    public string username { get; set; }
    public int diamond { get; set; }
}
