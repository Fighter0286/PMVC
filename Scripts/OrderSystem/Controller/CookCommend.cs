using System.Collections;
using System.Collections.Generic;
using OrderSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

public class CookCommend : SimpleCommand
{
   
    public override void Execute(INotification notification)
    {
        CookProxy cookProxy = Facade.RetrieveProxy(CookProxy.NAME) as CookProxy; //厨师的代理
        Order order = notification.Body as Order;
        if(notification.Type == "KAISHI")
        {
            cookProxy.CookCooking(order);
        }
        else if(notification.Type == "ZUOCAI")
        {
            cookProxy.CookCookingBuQueue(order);
        }
    }
}
