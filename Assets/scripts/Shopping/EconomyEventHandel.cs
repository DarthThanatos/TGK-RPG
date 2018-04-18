using UnityEngine;

public class EconomyEventHandel : MonoBehaviour {

    public delegate void BalanceHandler(int amount);
    public static event BalanceHandler OnBalanceChanged;

    public delegate void PlayerGoldHandler();
    public static event PlayerGoldHandler OnPlayerGoldChanged;

    public static void BalanceChanged(int amount)
    {
        if(OnBalanceChanged != null)
        {
            OnBalanceChanged(amount);
        }
    }

    public static void PlayerGoldChanged()
    {
        if(OnPlayerGoldChanged != null)
        {
            OnPlayerGoldChanged();
        }
    }

}
