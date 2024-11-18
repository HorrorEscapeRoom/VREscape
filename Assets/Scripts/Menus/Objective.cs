using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Objective
{
    public int id;
    public string title;
    public string text;

    public Objective(int id, string title, string text)
    {
        this.id = id;
        this.title = title;
        this.text = text;
    }
}
