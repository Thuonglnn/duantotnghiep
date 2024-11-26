using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginReponseModel
{
    public LoginReponseModel(int status, string message, string username, string name, List<string> generalIds, int score, int diamond, string itemId, string weaponId)
    {
        this.status = status;
        this.message = message;
        this.username = username;
        this.name = name;
        this.generalIds = generalIds;
        this.score = score;
        this.diamond = diamond;
        this.itemId = itemId;
        this.weaponId = weaponId;
    }

    public int status { get; set; }
    public string message { get; set; }
    public string username { get; set; }
    public string name { get; set; }

    public List<string> generalIds { get; set; }
    public int score { get; set; }
    public int diamond { get; set; }
    public string itemId { get; set; }
    public string weaponId { get; set; }


}
