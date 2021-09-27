using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KayarMenu : MonoBehaviour
{
    public GameObject Merkez, kontrolmerkezi;
    public ScrollRect KaymaMenu;
    public float PositionSonY;
    private void Start()
    {
       
    }
    void Update()
    {
   
        if (Merkez.GetComponent<RectTransform>().anchoredPosition.y > -200 && Merkez.GetComponent<RectTransform>().anchoredPosition.y < PositionSonY)
        {
            KaymaMenu.movementType = ScrollRect.MovementType.Unrestricted;
        }

        else if (Merkez.GetComponent<RectTransform>().anchoredPosition.y < -200 )
        {
            KaymaMenu.movementType = ScrollRect.MovementType.Elastic;
        }

        else if (Merkez.GetComponent<RectTransform>().anchoredPosition.y > PositionSonY)
        {
            KaymaMenu.movementType = ScrollRect.MovementType.Elastic;
        }
    }
}
