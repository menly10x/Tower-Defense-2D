﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    public static PlayerSetting instance;

    private float coin;
    public float Coin
    {
        get
        {
            return coin;
        }
        set
        {
            coin = value;
            UIController.instance.txtWood.text = coin.ToString();
        }
    }

    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            UIController.instance.txtHealth.text = health.ToString();
            Debug.Log(health);
            if (health == 0)
            {
                MonsterSpawnController.instance.StopAllCoroutines();
                MonsterSpawnController.instance.isDoneSpawn = false;
                UIController.instance.OpenPanelLose();
            }
        }
    }

    private string highScoreKey = "highScore";
    private int highScore;
    public int HighScore
    {
        get
        {
            if (!PlayerPrefs.HasKey(highScoreKey))
            {
                PlayerPrefs.SetInt(highScoreKey, 0);
            }

            highScore = PlayerPrefs.GetInt(highScoreKey);
            return highScore;
        }
        set
        {
            highScore = value;
            PlayerPrefs.SetInt(highScoreKey, highScore);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Coin = 200;
        Health = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
