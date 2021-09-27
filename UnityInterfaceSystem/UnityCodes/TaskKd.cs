using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskKd : MonoBehaviour
{
    public GameObject EntegreEdilenNesne;
    int b;
    public bool def;

    void Start()
    {

    }


    void Update()
    {
        if (def == true)
        {
            if (EntegreEdilenNesne.GetComponent<TakSokAnKod>().a == 1 && b == 0)
            {
                basari();
                b = 1;
            }
        }
    }

    public void TaskTiklama()
    {
        //Debug.Log(EntegreEdilenNesne.name);
    }

    public void basari()
    {
        transform.parent.transform.parent.GetComponent<Step>().basari++;
    }
}
