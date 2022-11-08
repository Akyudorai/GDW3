using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats
{
    public static bool CheatsEnabled = false;

    public class Teleport_Apartments : ICommand 
    {
        public void Execute() 
        {
            GameManager.GetInstance().RespawnPlayer(0);
        }
    }

    public class Teleport_Parking : ICommand 
    {
        public void Execute() 
        {
            GameManager.GetInstance().RespawnPlayer(1);
        }
    }

    public class Teleport_Mall : ICommand 
    {
        public void Execute() 
        {
            GameManager.GetInstance().RespawnPlayer(2);
        }
    }

    public class Teleport_Conyard : ICommand 
    {
        public void Execute() 
        {
            GameManager.GetInstance().RespawnPlayer(3);
        }
    }

    public class Teleport_Underpass : ICommand 
    {
        public void Execute() 
        {
            GameManager.GetInstance().RespawnPlayer(4);
        }
    }    

    public class Sunrise : ICommand
    {   
        // Set Time of Day to Sunrise
        public void Execute() 
        {
            Debug.Log("Sunrise");
            TimeManager.GetInstance().SetTimeOfDay(5.5f);
        }
    }

    public class Noon : ICommand
    {   
        // Set Time of Day to Noon
        public void Execute() 
        {
            Debug.Log("Noon");
            TimeManager.GetInstance().SetTimeOfDay(12);
        }
    }

    public class Sunset : ICommand
    {   
        // Set Time of Day to Sunset
        public void Execute() 
        {
            Debug.Log("Sunset");
            TimeManager.GetInstance().SetTimeOfDay(17.5f);
        }
    }

    public class Midnight : ICommand
    {   
        // Set Time of Day to Midnight
        public void Execute() 
        {
            Debug.Log("Midnight");
            TimeManager.GetInstance().SetTimeOfDay(0);
        }
    }

    public class GiveMoney : ICommand 
    {
        // Give the player 1000 money
        public void Execute() 
        {
            Debug.Log("Gained 1000 Money!");
            GameManager.GetInstance().playerRef.AddMoney(1000);            
            //ChangeScore.AddPoint();
        }
    }

    public class TakeMoney : ICommand
    {   
        // Remove 1000 money from the player
        public void Execute() 
        {
            Debug.Log("Lost 1000 Money!");
            GameManager.GetInstance().playerRef.RemoveMoney(1000);
            //ChangeScore.SubtractPoint();
        }
    }
}
