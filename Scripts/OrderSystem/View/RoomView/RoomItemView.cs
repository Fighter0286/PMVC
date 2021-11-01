using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItemView : MonoBehaviour
{
    private Text text = null;
    private Image image = null;
    public Roomltem roomltem = null;
    public IList<Action<object>> actionList = new List<Action<object>>();

    private void Awake()
    {
        text = transform.Find("Id").GetComponent<Text>();
        image = transform.GetComponent<Image>();
    }
    public void InitClient(Roomltem room)
    {
        this.roomltem = room;
        UpdateState();
    }

    private void UpdateState()
    {
        if (roomltem == null)
        {
            return;
        }
        Color color = Color.white;
        if (this.roomltem.state.Equals(0))
        {
            color = Color.green;
            //StartCoroutine(WaitingMenu());
        }
        else if (this.roomltem.state.Equals(1))
        {
            color = Color.yellow;
            //StartCoroutine(Serving());
        }

        else if (this.roomltem.state.Equals(2))
        {
            color = Color.red;
            StartCoroutine(eatting());
        }
        else if (this.roomltem.state.Equals(3))
        {
            StartCoroutine(AddGuest());
        }
        Debug.Log(roomltem.ToString());
        image.color = color;
        text.text = roomltem.ToString();

    }
    IEnumerator Serving(float time = 4)
    {
        yield return new WaitForSeconds(time);
        actionList[roomltem.state].Invoke(roomltem);
    }

    IEnumerator WaitingMenu(float time = 4)
    {
        yield return new WaitForSeconds(time);
        actionList[roomltem.state].Invoke(roomltem);
    }

    /// <summary>
    /// 来客人了
    /// </summary>
    /// <returns></returns>
    IEnumerator AddGuest(float time = 4)
    {
        yield return new WaitForSeconds(time);
        actionList[0].Invoke(roomltem);

    }
    private IEnumerator eatting(float time = 5)
    {
        Debug.Log(roomltem.id + "号桌客人正在就餐");
        yield return new WaitForSeconds(time);
        roomltem.state++;
        Debug.Log(roomltem.id + "客人离开饭店");
        actionList[1].Invoke(roomltem);
    }
}
