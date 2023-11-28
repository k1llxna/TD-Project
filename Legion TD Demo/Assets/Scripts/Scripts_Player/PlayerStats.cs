using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money; // static vars transfer between scenes
    public int startMoney = 400;

    public static int Lives;
    public int lives;
    public static int Rounds;

    void Start()
    {
        Money = startMoney;
        Lives = lives;

        Rounds = 0;
    }
}
