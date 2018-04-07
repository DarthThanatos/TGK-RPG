using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ItemDatabase : MonoBehaviour {
    public static ItemDatabase instance { get; set; }
    private List<Item> items { get; set; }

    // Use this for initialization
    void Awake () {
		if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        BuildDatabase();
	}
	

    private void BuildDatabase()
    {

        items = JsonConvert.DeserializeObject<List<Item>>(Resources.Load<TextAsset>("JSON/items").ToString());
        Debug.Log(items[0].Stats[0].StatName + " lvl is " + items[0].Stats[0].GetCalulatedStatValue());
    }


    public Item GetItem(string objectSlug)
    {
        return items.Find(x => x.ObjectSlug == objectSlug);
    }
}
