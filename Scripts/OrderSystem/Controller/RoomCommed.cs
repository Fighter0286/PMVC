using OrderSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCommed : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        RoomProxy roomProxy = Facade.RetrieveProxy(RoomProxy.NAME) as RoomProxy; //客房的代理
        if (notification.Type == "RUZHU")
        {
            roomProxy.Rooming(notification.Body as ClientItem);
        }
        else if (notification.Type == "LIKAI")
        {
            roomProxy.Roomlive(notification.Body as Roomltem);
        }
        else if(notification.Type == "CallClient")
        {
            roomProxy.ClientInRoom(notification.Body as Roomltem);
        }
        else if (notification.Type == "SelectRuZhu")
        {
            roomProxy.RoomOning(notification.Body as Roomltem);
        }
    }
}
