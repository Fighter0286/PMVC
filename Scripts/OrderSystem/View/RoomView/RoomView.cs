using OrderSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 房间ui层，用于更新ui，刷新状态
/// </summary>
public class RoomView : MonoBehaviour
{
    public UnityAction<Roomltem> CallWaiter = null;
    public UnityAction<Order> Order = null;
    public UnityAction Pay = null;
    public UnityAction CallCook = null;
    public UnityAction<Roomltem> ServerFood = null;

    private ObjectPool<RoomItemView> objectPool = null;
    private List<RoomItemView> rooms = new List<RoomItemView>();
    private Transform parent = null;

    private void Awake()
    {
        parent = this.transform.Find("Content");
        var prefab = Resources.Load<GameObject>("Prefabs/UI/RoomItem");
        objectPool = new ObjectPool<RoomItemView>(prefab, "RoomPool");
    }
    public void UpdateRoom(IList<Roomltem> rooms)
    {
        for (int i = 0; i < this.rooms.Count; i++)
            objectPool.Push(this.rooms[i]);

        this.rooms.AddRange(objectPool.Pop(rooms.Count));
        for (int i = 0; i < this.rooms.Count; i++)
        {
            var client = this.rooms[i];
            client.InitClient(rooms[i]);
            client.GetComponent<Button>().onClick.RemoveAllListeners();
            client.GetComponent<Button>().onClick.AddListener(() => {
                Debug.Log("111");
                if (client.roomltem.state == 0) 
                    CallWaiter(client.roomltem);
            });
        }
        Move(rooms);
    }

    public void Move(IList<Roomltem> rooms)
    {
        for (int i = 0; i < this.rooms.Count; i++)
        {
            this.rooms[i].transform.SetParent(parent);
            var item = rooms[i];
            this.rooms[i].transform.Find("Id").GetComponent<Text>().text = item.ToString();
            Color color = Color.white;
            if (item.state.Equals(0))
                color = Color.green;
            else if (item.state.Equals(1))
            {
                color = Color.yellow;
                StartCoroutine(WaiterServing(item, true));
            }
            else if (item.state.Equals(2))
                color = Color.red;
            this.rooms[i].GetComponent<Image>().color = color;
        }
    }

    public void Move(Roomltem rooms)
    {
        this.rooms[rooms.id - 1].transform.Find("Id").GetComponent<Text>().text = rooms.ToString();
        Color color = Color.white;
        if (rooms.state.Equals(0))
            color = Color.green;
        else if (rooms.state.Equals(1))
        {
            color = Color.blue;
            StartCoroutine(WaiterServing(rooms, true));
        }
        this.rooms[rooms.id - 1].GetComponent<Image>().color = color;
    }


    IEnumerator WaiterServing(Roomltem item, bool flag, float time = 5)
    {
        yield return new WaitForSeconds(time);
        ServerFood.Invoke(item);
    }
}
