using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private int money;

    public void AddMoney(int amount) 
    {
        money += amount;
        UI_Manager.GetInstance().SetMoneyDisplay(money);
    }

    public void RemoveMoney(int amount) 
    {
        money -= amount;
        UI_Manager.GetInstance().SetMoneyDisplay(money);
    }

    public int GetMoney() 
    {
        return money;
    }
}
