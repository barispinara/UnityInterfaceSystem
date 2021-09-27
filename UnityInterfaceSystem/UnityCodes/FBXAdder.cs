using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class FBXAdder : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        string current_path = Directory.GetCurrentDirectory();
        string[] current_path_splitter = current_path.Split('\\');

        string txt_path = current_path_splitter[0] + "\\" + current_path_splitter[1] + "\\" +current_path_splitter[2] + "\\bin\\Release\\UnitySender.txt";
        string[] lines = File.ReadAllLines(txt_path);
        
        
        

        foreach (string line in lines)
        {
            string[] line_splitter = line.Split('|');
            if (line_splitter[0].Equals("Model"))
            {
                string assets_path = line_splitter[1] + "/" + line_splitter[1];
                //Object meshes = Resources.Load(assets_path);
                //Assets / Resources / LenovoYoga710 / LenovoYoga710_3D_070621.blend
                GameObject meshes = (GameObject)AssetDatabase.LoadMainAssetAtPath("Assets/Resources/LenovoYoga710/LenovoYoga710.blend");
                
                GameObject t = GameObject.Instantiate(meshes, new Vector3(0, 0, 0), Quaternion.identity);
                t.name = line_splitter[1];


                t.transform.SetParent(GameObject.Find("Model").transform);
                t.transform.localPosition = new Vector3(0, 0, 0);
                t.transform.localScale = new Vector3(0.2780001f, 0.2780001f, 0.2780001f);
                t.transform.localRotation = Quaternion.Euler(90, 270, 0);
                t.AddComponent<Animation>();
                t.AddComponent<Animator>();
               

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
