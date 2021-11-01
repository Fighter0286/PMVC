using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomltem
{
    public int id { get; set; }
    public string name { get; set; }
    public int state { get; set; }
    public int peoplenum { get; set; }

    public Roomltem(int id, string name, int state,int peoplenum)
    {
        this.id = id;
        this.name = name;
        this.state = state;
        this.peoplenum = peoplenum;
    }

    public override string ToString()
    {
        return id + "号房间\n" + name + "\n" + resultState();
    }
    private string resultState()
    {
        if (state.Equals(0))
            return "房间已空";
        return "房间已满：" + id +"人数："+ peoplenum;
    }
}
