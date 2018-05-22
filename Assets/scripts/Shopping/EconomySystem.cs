using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomySystem : MonoBehaviour {

    public static EconomySystem instance { get; set; }

    private int _playerMoney;
    public int PlayerMoney {
        get { return _playerMoney; }
        set {
            _playerMoney = value;
            EconomyEventHandel.PlayerGoldChanged();
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }


    void Start () {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        PlayerMoney = 2500;
    }
	

}
