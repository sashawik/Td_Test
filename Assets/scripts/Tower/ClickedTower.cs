using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickedTower : MonoBehaviour,IPointerClickHandler
{
    public GameManager manager;
    public int type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (type < manager.Towers.Length) manager.Build(type, GetComponent<Image>().sprite);
        else manager.Sell();
    }
}
