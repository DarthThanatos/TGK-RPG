
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour {

    private GameObject healthBar;
    private Image currentHealthImage, maxHealthImage;

    private GameObject creature;
    private int currentHealth, maxHealth;

    void Start()
    {
        healthBar = Instantiate(Resources.Load<GameObject>("UI/Healthbar"));
        currentHealthImage = healthBar.transform.Find("ActualHP").GetComponent<Image>();
        maxHealthImage = healthBar.transform.Find("Background").GetComponent<Image>();
    }

    public void UpdateHealthBar(GameObject creature, int currentHealth, int maxHealth)
    {
        this.creature = creature;
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
    }

    void OnGUI()
    {
        Vector2 targetPos;
        targetPos = Camera.main.WorldToScreenPoint(creature.transform.position);
        Rect rect = new Rect(targetPos.x - 30, Screen.height - targetPos.y - 30, 60, 20);
        Rect curHealtRect = new Rect(targetPos.x - 30, Screen.height - targetPos.y - 30, 60 * (float)currentHealth / maxHealth, 20);

        GUI.DrawTexture(rect, maxHealthImage.mainTexture);
        GUI.DrawTexture(curHealtRect, currentHealthImage.mainTexture);
        GUI.Box(rect, currentHealth + "/" + maxHealth);

    }
}
