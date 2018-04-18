
using UnityEngine;
using UnityEngine.UI;

public class GoldUI : MonoBehaviour {

    private Text amountDisplayText;

	void Start () {
        amountDisplayText = Instantiate(Resources.Load<Text>("UI/StaticText"));
    }

    public void SetDisplayedAmount(int amount)
    {
        amountDisplayText.text = "Gold(" + amount.ToString() + ")";
    }

    void OnGUI()
    {
        Vector2 targetPos;
        targetPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Rect rect = new Rect(targetPos.x - 30, Screen.height - targetPos.y - 30, 80, 20);
        GUI.Box(rect, amountDisplayText.text);

    }

    void OnDestroy()
    {
        Destroy(amountDisplayText.gameObject);
    }
}
