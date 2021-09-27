using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TakSokBt : MonoBehaviour
{
    public Text Text;
    public int durum;
    public GameObject model;

    public void TakSokd()
    {

        if (durum == 0)
        {
            durum = 1;
            model.GetComponent<TakSokAnKod>().durum = durum;
            model.GetComponent<TakSokAnKod>().CikartmaAn();
            transform.parent.gameObject.SetActive(false);
        }
        else if (durum == 1)
        {
            durum = 0;
            model.GetComponent<TakSokAnKod>().durum = durum;
            model.GetComponent<TakSokAnKod>().TakmaAn();
            transform.parent.gameObject.SetActive(false);
        }
        
    }

    private void Update()
    {
        if (durum == 0)
        {
            Text.text = "Çıkart";
        }
        else if (durum == 1)
        {
            Text.text = "Tak";
        }
    }
}
