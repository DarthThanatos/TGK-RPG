using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogOptionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    void Start()
    {
        gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.red; 

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.white; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.red; 
    }
}
