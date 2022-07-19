using System.Collections;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
