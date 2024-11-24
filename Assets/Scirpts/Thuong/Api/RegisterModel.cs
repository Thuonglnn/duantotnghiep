using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterModel
{
    public RegisterModel(string username, string name, string password)
    {
        this.username = username;
        this.name = name;
        this.password = password;
    }

    public string username { get; set; }
    public string name { get; set; }
   public string password { get; set; }
   
 

}
