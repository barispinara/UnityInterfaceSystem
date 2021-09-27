using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Step : MonoBehaviour
{
    public GameObject Tasklar, Ok, sontask, kontrolnoktasi;
    public GameObject[] SteplerS;
    public Sprite KapaliOk, AcikOk, Arti;
    public bool AcikKapali;
    public int basari;
    public int olmasiGerekenBasari;

    int a;
    void Start()
    {
        SteplerS = GameObject.FindGameObjectsWithTag("Step");
    }
    public void basma()
    {


        if (AcikKapali == false)
        {
            for (int a = 0; a < SteplerS.Length; a++)
            {
                if (SteplerS[a].GetComponent<Step>().AcikKapali == true)
                {

                    SteplerS[a].GetComponent<Step>().basma();
                }
            }

            AcikKapali = true;
            Tasklar.transform.localScale = new Vector3(1, 1, 1);
            if (basari != olmasiGerekenBasari)
            {
                Ok.GetComponent<Image>().sprite = AcikOk;
            }
        }
        else if (AcikKapali == true)
        {
            AcikKapali = false;
            Tasklar.transform.localScale = new Vector3(1, 0, 1);
            if (basari != olmasiGerekenBasari)
            {
                Ok.GetComponent<Image>().sprite = KapaliOk;
            }
        }
       
    }


     void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="task")
        {
            sontask = collision.gameObject.transform.parent.gameObject.transform.GetChild(collision.gameObject.transform.parent.gameObject.transform.childCount-1).gameObject;
            //GetComponent<RectTransform>().anchoredPosition = new Vector3(0, sontask.GetComponent<RectTransform>().anchoredPosition.y-70, 0);
        }

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="task")
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GetComponent<RectTransform>().anchoredPosition, new Vector3(0, sontask.GetComponent<RectTransform>().anchoredPosition.y - 450, 0), Time.deltaTime * 0.1f);
        }

    }


    void Update()
    {
        if (basari == olmasiGerekenBasari && a == 0)
        {
            var tempColor = GetComponent<Image>().color;
            tempColor.a = 0.5f;
            GetComponent<Image>().color = tempColor;

            for (int i = 0; i < transform.GetChild(2).transform.childCount; i++)
            {
                var tempColor2 = transform.GetChild(2).transform.GetChild(i).GetComponent<Image>().color;
                tempColor2.a = 0.5f;
                transform.GetChild(2).transform.GetChild(i).GetComponent<Image>().color = tempColor2;
            }
            AcikKapali = true;
            basma();
            Ok.GetComponent<Image>().sprite = Arti;
            a = 1;
        }
        else if (basari != olmasiGerekenBasari && a == 1)
        {
       
            var tempColor3 = GetComponent<Image>().color;
            tempColor3.a = 1f;
            GetComponent<Image>().color = tempColor3;

            for (int b = 0; b < transform.GetChild(2).transform.childCount; b++)
            {
                var tempColor4 = transform.GetChild(2).transform.GetChild(b).GetComponent<Image>().color;
                tempColor4.a = 1f;
                transform.GetChild(2).transform.GetChild(b).GetComponent<Image>().color = tempColor4;
            }

            basma();
            a = 0;
        }

    }

}
