
/*=========================================
* Author: Administrator
* DateTime:2017/6/21 18:17:11
* Description:$safeprojectname$
==========================================*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using PureMVC.Patterns;

namespace OrderSystem
{
    public class CookProxy : Proxy
    {
        public new const string NAME = "CookProxy";
        public Queue<Order> orders;  //厨师队列
        public bool flag = false;
        public int count = 0;
        public IList<CookItem> Cooks
        {
            get { return (IList<CookItem>)base.Data; }
        }

        public CookProxy() : base(NAME, new List<CookItem>())
        {
            orders = new Queue<Order>();
            AddCook(new CookItem(1, "强尼", 0));
            AddCook(new CookItem(2, "托尼"));
            AddCook(new CookItem(3, "鲍比", 0));
            AddCook(new CookItem(4, "缇米"));
        }
        public void AddCook(CookItem item)
        {
            Cooks.Add(item);
        }
        public void RemoveCook(CookItem item)
        {
            Cooks.Remove(item);
        }

        public void CookCooking(Order order)
        {
            count = 0;
            for (int i = 0; i < Cooks.Count; i++)
            {
                if (Cooks[i].state != 0)//找到非忙碌厨师改变其状态
                {
                    count += 1;
                }
            }
            if (count == 4)
            {
                flag = true;
            }
            if (flag)
            {
                orders.Enqueue(order);
                flag = false;
            }
            for (int i = 0; i < Cooks.Count; i++)
            {
                if (Cooks[i].state == 0)//找到非忙碌厨师改变其状态
                {
                    Cooks[i].state++;
                    Cooks[i].cooking = order.names;//厨师抄的什么菜
                    Cooks[i].cookOrder = order;// 厨师炒菜的菜单
                    UnityEngine.Debug.Log(order.names);
                    SendNotification(OrderSystemEvent.ResfrshCook);//找到空闲厨师去刷新一下厨师显示的状态
                    return;
                }
            }
        }
        public void CookCookingBuQueue(Order order)
        {
            UnityEngine.Debug.Log("初始检查队列");
            UnityEngine.Debug.Log(orders.Count);
            if (orders.Count > 0)
            {
                Order order1 = orders.Dequeue();
                SendNotification(OrderSystemEvent.CookCooking, order1, "KAISHI");  //开始做菜
            }
        }
    }
}