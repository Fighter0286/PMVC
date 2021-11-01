using OrderSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 房间ui 逻辑层
/// </summary>
public class RoomMediator : Mediator
{
    private RoomProxy roomProxy = null;
    public new const string NAME = "RoomMediator";
    public RoomView RoomView
    {
        get { return (RoomView)base.ViewComponent; }
    }

    public RoomMediator(RoomView view) : base(NAME, view)
    {
        //绑定消息委托
        RoomView.CallWaiter += (roomitem) =>
        {
            //发消息，通知客人入住
            SendNotification(OrderSystemCommend.selectRoomNUM, roomitem, "CallClient");
        };
        RoomView.Order += data => { };
        RoomView.Pay += () => { };
        RoomView.CallCook += () => { };
        RoomView.ServerFood += roomitem =>
        {
            SendNotification(OrderSystemCommend.selectRoomNUM, roomitem, "LIKAI");
        };
    }
    //刷新
    public override void OnRegister()
    {
        base.OnRegister();
        roomProxy = Facade.RetrieveProxy(RoomProxy.NAME) as RoomProxy;
        if (null == roomProxy)
            throw new Exception(RoomProxy.NAME + "is null,please check it!");
        if (null == roomProxy)
            throw new Exception(OrderProxy.NAME + "is null,please check it!");
        RoomView.UpdateRoom(roomProxy.Rooms);
    }

    //感兴趣的消息列表
    public override IList<string> ListNotificationInterests()
    {
        IList<string> notifications = new List<string>();
        notifications.Add(OrderSystemEvent.selectRoom);
        notifications.Add(OrderSystemEvent.ResfrshRoom);
        notifications.Add(OrderSystemEvent.Room_Leave);
        return notifications;
    }
    //switch功能区域，用于拓展
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case OrderSystemEvent.selectRoom:
                ClientItem client = notification.Body as ClientItem;
                //客房部收到客人就餐完毕，选择空闲房间入住
                Debug.Log("客房部收到客人就餐完毕，选择空闲房间入住");
                SendNotification(OrderSystemCommend.selectRoomNUM, client, "RUZHU");
                break;
            case OrderSystemEvent.ResfrshRoom:
                Roomltem Item = notification.Body as Roomltem;
                //刷新客房状态
                RoomView.Move(Item);//刷新一下服务员的状态
                break;
            case OrderSystemEvent.Room_Leave:
                //客人离开

                break;

        }
    }
}
