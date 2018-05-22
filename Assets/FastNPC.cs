using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastNPC : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Texture2D originalTexture = (Texture2D)GetComponent<Renderer>().material.mainTexture;

        Texture2D targetTexture = new Texture2D(originalTexture.width, originalTexture.height);
        GetComponent<Renderer>().material.mainTexture = targetTexture;

        for (int y = 0; y < originalTexture.height; y++)
        {
            for (int x = 0; x < originalTexture.width; x++)
            {

                targetTexture.SetPixel(x, y, Color.green);

            }
        }

        targetTexture.Apply();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
