using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

public class TakSokAnKod : MonoBehaviour
{
    public GameObject TakSokGO, TakSokBt, kontrolnoktasi;
    public Animator An;
    public string TakmaAnimasyonIsim, CikartmaAnimasyonIsim;
    public int durum, sonmat;
    public int a, Siralama;
    public Material SecilenMaterial;
    Ray ray;
    RaycastHit hit;
    MeshRenderer meshRenderer;
    Material[] materials;
    Material[] materials2; 
     void Start()
    {


        meshRenderer = GetComponent<MeshRenderer>();
        
        materials = meshRenderer.materials;
        materials2 = meshRenderer.materials;
        var obj = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("hidden"));
        var obj2 = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("hidden2"));
        TakSokGO = obj as GameObject;
        TakSokBt = obj2 as GameObject;

        string current_path = Directory.GetCurrentDirectory();
        string[] current_path_splitter = current_path.Split('\\');

        string txt_path = current_path_splitter[0] + "\\" + current_path_splitter[1] + "\\" + current_path_splitter[2] + "\\bin\\Release\\UnitySender.txt";
        string[] lines = File.ReadAllLines(txt_path);

        foreach (string line in lines)
        {
            string[] line_splitter = line.Split('|');
            if (line_splitter[0].Equals("Model"))
            {
                An = GameObject.Find(line_splitter[1]).GetComponent<Animator>();
            }

            //An = GameObject.Find("LenovoYoga710(Clone)").GetComponent<Animator>();
        }
    }
    void Update()
    {

        if (Siralama == kontrolnoktasi.GetComponent<ModelAyristirma>().siralama)
        {
            for(int i = 0; i< materials2.Length; i++)
            {
                materials2[i] = SecilenMaterial;
            }
            
            meshRenderer.materials = materials2;
        }
        if (Siralama != kontrolnoktasi.GetComponent<ModelAyristirma>().siralama)
        {
           
        }
       

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log(hit.collider.name);

            if (Siralama == kontrolnoktasi.GetComponent<ModelAyristirma>().siralama)
            {

                
                if (Input.GetMouseButtonDown(0))
                {


                    
                    if (hit.collider.name == gameObject.name)
                    {
                  
                        TakSokBt.GetComponent<TakSokBt>().model = hit.collider.gameObject;
                        TakSokGO.SetActive(true);
                        
                        TakSokGO.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(hit.point);
                        TakSokBt.GetComponent<TakSokBt>().durum = durum;
                    }
                    else
                    {
                        TakSokGO.SetActive(false);
                    }
                }
            }
        }

        
    }


    public void TakmaAn()
    {
        An.enabled = true;
        An.Play(TakmaAnimasyonIsim);
        kontrolnoktasi.GetComponent<ModelAyristirma>().devamedenanimasyonsayisi++;
        kontrolnoktasi.GetComponent<ModelAyristirma>().siralama++;
        //mater = null;
        meshRenderer.materials = materials;
        GetComponent<TakSokAnKod>().enabled = false;
        a = 1;
   
    }
    public void CikartmaAn()
    {
        An.enabled = true;
        An.Play(CikartmaAnimasyonIsim);
        kontrolnoktasi.GetComponent<ModelAyristirma>().devamedenanimasyonsayisi++;
        kontrolnoktasi.GetComponent<ModelAyristirma>().siralama++;
        //meshRenderer.materials[meshRenderer.materials.Length - 1] = null;
        meshRenderer.materials = materials;
        GetComponent<TakSokAnKod>().enabled = false;
        a = 1;
    }
}
