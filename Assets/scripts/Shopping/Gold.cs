
using UnityEngine;

public class Gold : Interactable {

    private GoldUI goldUI;

    private int _amount = -1;
    public int Amount {
        get { return _amount;  }
        set
        {
            _amount = value;
            if(goldUI != null)
                goldUI.SetDisplayedAmount(Amount);
        }
    }


    void Start()
    {
        goldUI = GetComponent<GoldUI>();
        Amount = _amount == -1 ? 20 + Random.Range(-5, 5) : _amount;
    }



    public override void Interact()
    {
        Debug.Log("Interacting with gold");
        EconomySystem.instance.PlayerMoney += Amount;
        Destroy(gameObject);
    }

}
