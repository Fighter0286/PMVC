
/*=========================================
* Author: Administrator
* DateTime:2017/6/20 19:21:23
* Description:$safeprojectname$
==========================================*/

using System;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

namespace OrderSystem
{
    internal class WaiterMediator : Mediator
    {
        private WaiterProxy waiterProxy = null;
        public new const string NAME = "WaiterMediator";
        public WaiterView WaiterView
        {
            get { return (WaiterView)base.ViewComponent; }
        }

        //todo 订单代理
        private OrderProxy orderProxy = null;

        public WaiterMediator(WaiterView view) : base(NAME, view)
        {
            WaiterView.CallWaiter += () => { SendNotification(OrderSystemEvent.CALL_WAITER); };
            WaiterView.Order += data => { SendNotification(OrderSystemEvent.ORDER, data); };
            WaiterView.Pay += () => { SendNotification(OrderSystemEvent.PAY); };
            WaiterView.CallCook += () => { SendNotification(OrderSystemEvent.CALL_COOK); };
            WaiterView.ServerFood += item =>
            {
                SendNotification(OrderSystemEvent.selectWaiter, item, "WANSHI"); //付完款之和将服务员状态变更
            };


        }

        public override void OnRegister()
        {
            base.OnRegister();
            waiterProxy = Facade.RetrieveProxy(WaiterProxy.NAME) as WaiterProxy;
            orderProxy = Facade.RetrieveProxy(OrderProxy.NAME) as OrderProxy;
            if (null == waiterProxy)
                throw new Exception(WaiterProxy.NAME + "is null,please check it!");
            if (null == orderProxy)
                throw new Exception(OrderProxy.NAME + "is null,please check it!");
            WaiterView.UpdateWaiter(waiterProxy.Waiters);
        }

        public override IList<string> ListNotificationInterests()
        {
            IList<string> notifications = new List<string>();
            notifications.Add(OrderSystemEvent.CALL_WAITER);
            notifications.Add(OrderSystemEvent.ORDER);
            notifications.Add(OrderSystemEvent.GET_PAY);
            notifications.Add(OrderSystemEvent.FOOD_TO_CLIENT);
            notifications.Add(OrderSystemEvent.ResfrshWarite);
            notifications.Add(OrderSystemCommend.DOWNORDER);
            return notifications;
        }
        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case OrderSystemEvent.CALL_WAITER:
                    ClientItem client = notification.Body as ClientItem;
                    Debug.Log("aaa");
                    SendNotification(OrderSystemEvent.GET_ORDER, client, "Get");//请求获取菜单的命令 GetAndExitOrderCommed
                    break;
                case OrderSystemEvent.ORDER:
                    //服务员下单命令
                    SendNotification(OrderSystemEvent.selectWaiter, notification.Body, "DownOrder");
                    //SendNotification(OrderSystemEvent.CALL_COOK , notification.Body);
                    break;
                case OrderSystemEvent.GET_PAY:
                    Debug.Log(" 服务员拿到顾客的付款 ");
                    ClientItem item = notification.Body as ClientItem;
                    // SendNotification(OrderSystemEvent.selectWaiter, item, "WANSHI"); //付完款之和将服务员状态变更
                    SendNotification(OrderSystemEvent.GUEST_BE_AWAY, item, "Remove");
                    break;
                case OrderSystemEvent.FOOD_TO_CLIENT:
                    Debug.Log(" 服务员上菜 ");
                    // Debug.Log(notification.Body.GetType());
                    WaiterItem waiterItem = notification.Body as WaiterItem;
                    waiterItem.order.client.state++;
                    SendNotification(OrderSystemEvent.PAY, waiterItem);
                    break;
                case OrderSystemEvent.ResfrshWarite:
                    // waiterProxy = Facade.RetrieveProxy(WaiterProxy.NAME) as WaiterProxy;
                    WaiterItem Item = notification.Body as WaiterItem;
                    WaiterView.Move(Item);//刷新一下服务员的状态
                    break;
            }
        }

    }
}