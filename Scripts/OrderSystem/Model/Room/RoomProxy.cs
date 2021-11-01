using OrderSystem;
using PureMVC.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomProxy : Proxy
{
    public new const string NAME = "RoomProxy";
    public Queue<int> rooms;  //房间队列
    //public bool flag = false;
    //public int count = 0;
    public IList<Roomltem> Rooms
    {
        get { return (IList<Roomltem>)base.Data; }
    }

    public RoomProxy() : base(NAME, new List<Roomltem>())
    {
        rooms = new Queue<int>();
        AddRook(new Roomltem(1, "总统房", 0,0));
        AddRook(new Roomltem(2, "海景房",0,0));
        AddRook(new Roomltem(3, "情侣房", 0,0));
        AddRook(new Roomltem(4, "标准房",0,0));
    }
    //点击房间
    public void ClientInRoom(Roomltem roomltem)
    {
        Debug.Log("进入房间");
          //发消息给客房部
       SendNotification(OrderSystemCommend.selectRoomNUM, roomltem, "SelectRuZhu");
    }

    public void AddRook(Roomltem item)
    {
        Rooms.Add(item);
    }
    //离开
    public void RemoveRoom(OrderSystem.ClientItem clientItem)
    {
            
    }

    //入住
    public void RoomOning(Roomltem roomltem)
    {
        if (roomltem!=null && rooms.Count>0)
        {
            Debug.Log("代理收到，开始办理入住");
            for (int i = 0; i < Rooms.Count; i++)
            {
                if (Rooms[i].state == 0 && roomltem.id == Rooms[i].id)
                {
                    //找到空房间，刷新状态
                    Rooms[i].state = 1;
                    int num = rooms.Dequeue();
                    Rooms[i].peoplenum = num;
                    Debug.Log("人数：：：：：：：：：：：：：：：：：：：：：："+ num);
                    SendNotification(OrderSystemEvent.ResfrshRoom, Rooms[i]);
                    return;
                }
            }
        }
    }

    //进入等待列表
    public void Rooming(OrderSystem.ClientItem clientItem)
    {
        //int count = 0;
        //foreach (var item in Rooms)
        //{
        //    if (item.state==1)
        //    {
        //        count++;
        //    }
        //}
        //if (count==4)
        //{
        //    rooms.Enqueue(clientItem);
        //    Debug.Log("房间都慢，加入队列");
        //}
        //Debug.Log("代理收到，开始办理入住");

        //for (int i = 0; i < Rooms.Count; i++)
        //{
        //    if (Rooms[i].state == 0)
        //    {
        //        //找到空房间，刷新状态
        //        Rooms[i].state = 1;
        //        Rooms[i].peoplenum = clientItem.population;
        //        SendNotification(OrderSystemEvent.ResfrshRoom,Rooms[i]);
        //        return;
        //    }
        //}
        int num = clientItem.population;
        rooms.Enqueue(num);
        Debug.Log("客人加入等待队列"+clientItem.id.ToString()+"  "+clientItem.population.ToString());
    }
    public void Roomlive(Roomltem roomltem)
    {
        Debug.Log("客人离开，刷新状态，检查队列");
        roomltem.state = 0;
        roomltem.peoplenum = 0;
        //刷新状态
        SendNotification(OrderSystemEvent.ResfrshRoom, roomltem);
        //if (rooms.Count>0)
        //{
        //    ClientItem clientItem = rooms.Dequeue();
        //    //发消息给客房部
        //    SendNotification(OrderSystemEvent.selectRoom, clientItem);
        //}
    }
}
