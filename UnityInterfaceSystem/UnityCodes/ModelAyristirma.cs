using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEditor;

public class ModelAyristirma : MonoBehaviour
{
    [Header("Model Sökülecek mı Takilacakmı(Bir diğeri açıkken diğerini kapat)")]
    //public bool Sokulecek;

    public bool Takilacak = true;

    [Header("Modeller Hareket ve Tek Model Ayarları")]
    public float ModelDondurmeHizi ;

    public float ModelYakinlastirmaHizi;
    public GameObject ModelTum;

    [Header("Animasyon Sayısı(Her Model için) KALDIRILDI")]
    public int AnimasyonSayisi = 0;
    [Header("(Bu Kısımı Elleme)")]
    public int devamedenanimasyonsayisi;

    [Header("Modellerin Parçaları Tasklarla Eşleşir --- OTOMATIK EKLENIYOR")]
    //public GameObject[] ModelParcalar;
    public List<GameObject> ModelParcalar = new List<GameObject>();

    [Header("Step Ve Task Ayarları")]
    public GameObject Step;

    public GameObject StepClone;
    public GameObject  Task;
    public GameObject TaskClone;
    public int StepHiz, TaskHiz;

    [Header("Toplam Step Sayısı")]
    //public GameObject[] Stepler;
    public int step_sayisi;
    //public List<GameObject> Stepler = new List<GameObject>();

    [Header("Toplam Task Sayısı")]
    //public GameObject[] Tasklar;
    //public List<GameObject> Tasklar = new List<GameObject>();
    public int task_sayisi;

    [Header("Step Ve Task İsimleri (Sırayla)")]
    //public string[] StepIsim;
    // public string[] TaskIsim;
    public List<string> StepIsim = new List<string>();
    public List<string> TaskIsim = new List<string>();

    [Header("Her Step Başına Düşen Task Sayısı (Sırayla)")]
    //public int[] TaskSayilari;
    public List<int> TaskSayilari = new List<int>();

    int sayac = 0, sayac2;
    float scrool = 1;
    public GameObject TamAnBt, kayarmenud;
    public Animator TamAnimasyonAnimator;
    public string TamAnimasyonIsim;
    public int siralama;

    /*-----------------------------------------------
     * 
     * 
     * File reading Kısmı
     * 
     * 
     * ---------------------------------------------------*/

    private List<StepINPUT> step_list = new List<StepINPUT>();
    private List<TaskINPUT> task_list = new List<TaskINPUT>();
    
    private void Input_Reading()
    {
        string current_path = Directory.GetCurrentDirectory();
        string[] current_path_splitter = current_path.Split('\\');
        string txt_path = current_path_splitter[0] + "\\" + current_path_splitter[1] + "\\" + current_path_splitter[2]+ "\\bin\\Release\\UnitySender.txt";
        string[] lines = File.ReadAllLines(txt_path);
        
        foreach (string line in lines)
        {
            
            /*---------------------------
             * StepINPUT için kullanılan splitter içerisinde;
             * [1] -> Sıra Numarası (1,2,3 vb.)
             * [2] -> Task'ın ismi
             * ----------------------------*/
            string[] splitter = line.Split('|');
            if (splitter[0].Equals("Task"))
            {
                List<TaskINPUT> task_list = new List<TaskINPUT>();
                StepINPUT step_input = new StepINPUT(splitter[1], splitter[2], task_list);
                step_list.Add(step_input);
                //Stepler.Add(splitter[1]);
                step_sayisi++;
            }
           

            /*------------------------
             * TaskINPUT için kullanılan splitter içerisinde;
             * [1] -> Ait olduğu Step'in adı
             * [2] -> Sıra Numarası (1.1 , 1.2 , 2.1 vb.)
             * [3] -> Task Adı 
             * [4] -> Animasyonun ait olduğu parçanın ismi
             * [5] -> Parçanın ait olduğu .fbx modelin adı
             * [6] -> Animasyonun başlangıç (start) frame'i
             * [7] -> Animasyonun bitiş (finish) frame'i
             * --------------------------*/
            else if (splitter[0].Equals("SubTask"))
            {
                TaskINPUT step_input = new TaskINPUT(splitter[1], splitter[2], splitter[3], splitter[4], splitter[5]);

                step_list[step_sayisi - 1].task_list.Add(step_input);
                task_list.Add(step_input);
                AnimasyonSayisi++;
                task_sayisi++;
            }


        }

    }


    public void Start()
    {
        Input_Reading();

        string current_path = Directory.GetCurrentDirectory();
        string[] current_path_splitter = current_path.Split('\\');
        string txt_path = current_path_splitter[0] + "\\" + current_path_splitter[1] + "\\" + current_path_splitter[2] + "\\bin\\Release\\UnitySender.txt";
        string[] lines = File.ReadAllLines(txt_path);


        Object[] fbxmodel = null;
        string model_name = "";
        foreach (string line in lines)
        {
            string[] line_splitter = line.Split('|');
            if (line_splitter[0].Equals("Model"))
            {
                fbxmodel = Resources.LoadAll(line_splitter[1]+"/"+line_splitter[1]);
                model_name = line_splitter[1];
                break;
            }
        }

        UnityEditor.AssetDatabase.DeleteAsset("Assets/Resources/" + model_name + "/Animasyon/" + model_name + ".controller");
        
        
        
        var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Resources/" + model_name + "/Animasyon/" + model_name + ".controller");
        var rootStateMachine = controller.layers[0].stateMachine;
        rootStateMachine.AddState("Pass");



        GameObject[] Stepler = new GameObject[step_sayisi];
        GameObject[] Tasklar = new GameObject[task_sayisi];



        /*AnimationClip newClip = new AnimationClip();
        AnimationClip orgclip = (AnimationClip)AssetDatabase.LoadAssetAtPath("Assets/Resources/LenovoYoga710/Animasyon/LenovoYoga710.anim", typeof(AnimationClip));
        EditorUtility.CopySerialized(orgclip, newClip);
        AnimationClipSettings clipSettings = new AnimationClipSettings();
        clipSettings.startTime = 0.0f;
        clipSettings.stopTime = 10.0f / orgclip.frameRate;
        AnimationUtility.SetAnimationClipSettings(newClip, clipSettings);*/

        /*AnimationClip newClip2 = new AnimationClip();
        AnimationClip orgclip2 = (AnimationClip)AssetDatabase.LoadAssetAtPath("Assets/Resources/LenovoYoga710/Animasyon/LenovoYoga710.anim", typeof(AnimationClip));
        EditorUtility.CopySerialized(orgclip2, newClip2);
        AnimationClipSettings clipSettings2 = new AnimationClipSettings();
        clipSettings2.startTime = (0 / orgclip2.frameRate);
        clipSettings2.stopTime = (170 / orgclip2.frameRate);
        AnimationUtility.SetAnimationClipSettings(newClip2, clipSettings2);
        AssetDatabase.CreateAsset(newClip2, "Assets/Resources/" + model_name + "/Animasyon/Starting.anim");
        rootStateMachine.AddState("Starting");
        rootStateMachine.states[root_state_counter].state.motion = Resources.Load("Assets/Resources/LenovoYoga710/Animasyon/Starting") as AnimationClip;
        root_state_counter++;*/



        
        for (int a = 0; a < step_list.Count; a++)
        {
            for(int b = 0; b < step_list[a].task_list.Count; b++) 
            {
                
                AnimationClip newClip = new AnimationClip();
                AnimationClip orgclip = (AnimationClip)AssetDatabase.LoadAssetAtPath("Assets/Resources/LenovoYoga710/Animasyon/LenovoYoga710.anim", typeof(AnimationClip));
                EditorUtility.CopySerialized(orgclip, newClip);
                AnimationClipSettings clipSettings = new AnimationClipSettings();
                clipSettings.startTime = (int.Parse(step_list[a].task_list[b].start) / orgclip.frameRate);
                clipSettings.stopTime = (int.Parse(step_list[a].task_list[b].finish) / orgclip.frameRate);
                AnimationUtility.SetAnimationClipSettings(newClip, clipSettings);
                AssetDatabase.CreateAsset(newClip, "Assets/Resources/" + model_name + "/Animasyon/" + step_list[a].task_list[b].piece_name+".anim");
            }
        }








        int root_state_counter = 1;
        for (int i = 0; i<task_list.Count; i++)
        {
            
            for(int x=0; x<fbxmodel.Length; x++)
            {
                if (task_list[i].piece_name.Contains(fbxmodel[x].name))
                {
                    
                    ModelParcalar.Add(GameObject.Find(fbxmodel[x].name));
                    GameObject.Find(fbxmodel[x].name).AddComponent<TakSokAnKod>();
                    GameObject.Find(fbxmodel[x].name).GetComponent<TakSokAnKod>().kontrolnoktasi = GameObject.Find("KontrolNoktasi");
                    GameObject.Find(fbxmodel[x].name).GetComponent<TakSokAnKod>().TakmaAnimasyonIsim = fbxmodel[x].name;
                    GameObject.Find(fbxmodel[x].name).AddComponent<BoxCollider>();
                    GameObject.Find(fbxmodel[x].name).AddComponent<MeshCollider>();
                    GameObject.Find(fbxmodel[x].name).GetComponent<MeshCollider>().convex = true;
                    
                    GameObject.Find(fbxmodel[x].name).GetComponent<TakSokAnKod>().durum = 1;
                    GameObject.Find(fbxmodel[x].name).GetComponent<TakSokAnKod>().sonmat = 0;
                    GameObject.Find(fbxmodel[x].name).GetComponent<TakSokAnKod>().Siralama = i;
                    GameObject.Find(fbxmodel[x].name).GetComponent<TakSokAnKod>().SecilenMaterial = Resources.Load(model_name + "/Secme") as Material;

                    if (task_list[i].piece_name.Contains("UnderCaseScrew"))
                    {
                        
                        GameObject.Find(fbxmodel[x].name).GetComponent<BoxCollider>().size = new Vector3(0.05506565f, 0.0563856f, 5f);
                    }

                    rootStateMachine.AddState(task_list[i].piece_name);
                    rootStateMachine.states[root_state_counter].state.motion = Resources.Load(task_list[i].model_name + "/Animasyon/" + task_list[i].piece_name) as AnimationClip;
                    root_state_counter++;
                    
                }
            }
        }


        //GameObject.Find("LenovoYoga710").GetComponent<Animator>().runtimeAnimatorController = Resources.Load(model_name + "/Animasyon/" + model_name) as RuntimeAnimatorController;
        
        /*for(int i = 2; i<fbxmodel.Length; i++)
        {
            ModelParcalar.Add(GameObject.Find(fbxmodel[i].name));
            GameObject.Find(fbxmodel[i].name).AddComponent<TakSokAnKod>();
            GameObject.Find(fbxmodel[i].name).GetComponent<TakSokAnKod>().kontrolnoktasi = GameObject.Find("KontrolNoktasi");
            GameObject.Find(fbxmodel[i].name).AddComponent<MeshCollider>();
            GameObject.Find(fbxmodel[i].name).GetComponent<MeshCollider>().convex = true;
            
            

            /*if (fbxmodel[i].name.Equals("Pistol_Body")){
                GameObject.Find(fbxmodel[i].name).GetComponent<TakSokAnKod>().TakmaAnimasyonIsim = "pimicekme";
            }
            else if (fbxmodel[i].name.Equals("Pistol_Hammer"))
            {
                GameObject.Find(fbxmodel[i].name).GetComponent<TakSokAnKod>().TakmaAnimasyonIsim = "Silahhavayakalkma";
            }
            else if (fbxmodel[i].name.Equals("Pistol_Magazine"))
            {
                GameObject.Find(fbxmodel[i].name).GetComponent<TakSokAnKod>().TakmaAnimasyonIsim = "Sarjor";
            }
            else if (fbxmodel[i].name.Equals("Pistol_Top"))
            {
                GameObject.Find(fbxmodel[i].name).GetComponent<TakSokAnKod>().TakmaAnimasyonIsim = "Silahicekme";
            }
            else if (fbxmodel[i].name.Equals("Pistol_Trigger"))
            {
                GameObject.Find(fbxmodel[i].name).GetComponent<TakSokAnKod>().TakmaAnimasyonIsim = "atesleme";
            }*/

        


        for (int a = 0; a<step_sayisi; a++)
        {
            StepIsim.Add(step_list[a].step_name);
            TaskSayilari.Add(step_list[a].task_list.Count);
            for(int x=0; x<step_list[a].task_list.Count; x++)
            {
                TaskIsim.Add(step_list[a].task_list[x].step_name);
            }
        }

        Step = GameObject.Find("Step1");
        Task = GameObject.Find("Task");

        TamAnBt = GameObject.Find("TamAnimasyon");
        kayarmenud = GameObject.Find("ArkaPlan (1)");
        TamAnimasyonAnimator = GameObject.Find("LenovoYoga710").GetComponent<Animator>();
        TamAnimasyonIsim = "atesleme";

        /*if(Sokulecek==true)
        { 
            for(int a=0;a<ModelParcalar.Length;a++)
            {
                ModelParcalar[a].GetComponent<TakSokAnKod>().durum = 0;
                ModelParcalar[a].GetComponent<TakSokAnKod>().Siralama = a;
            }
        }*/

        /*if (Takilacak==true)
        {
            for (int a = 0; a < ModelParcalar.Count; a++)
            {
                ModelParcalar[a].GetComponent<TakSokAnKod>().durum = 1;
                ModelParcalar[a].GetComponent<TakSokAnKod>().Siralama = a;
                ModelParcalar[a].GetComponent<TakSokAnKod>().Siralama = a;
            }
        }*/

        Stepler[0] = GameObject.Find("Step1");
        Stepler[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = 1 + "." + StepIsim[0];
        Stepler[0].GetComponent<Step>().olmasiGerekenBasari = TaskSayilari[0];

        Tasklar[0] = GameObject.Find("Task");
        Tasklar[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = 1 + "." + TaskIsim[0];

        StepHiz = -20;
        TaskHiz = -20;

        if (Stepler.Length > 1)
        {
            for (int i = 1; i < Stepler.Length; i++)
            {
                StepClone = Instantiate(Step);
                StepClone.transform.GetChild(2).name = "Tasklar" + (i + 1);
                Stepler[i] = StepClone;
                Stepler[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1) + "." + StepIsim[i];
                StepClone.transform.parent = GameObject.Find("ArkaPlan").transform;
                StepClone.transform.localScale = new Vector3(1, 1, 1);
                StepClone.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -60 * (i + 1), 0);
                StepClone.GetComponent<Rigidbody2D>().gravityScale = StepHiz;
                StepClone.GetComponent<HingeJoint2D>().enabled = false;
                StepClone.GetComponent<Step>().olmasiGerekenBasari = TaskSayilari[i];
            }
        }
        for (int b = 0; b < Stepler.Length; b++)
        {
            sayac++;

            for (int a = 0; a < TaskSayilari[b]; a++)
            {
                TaskClone = Instantiate(Task);
                TaskClone.GetComponent<TaskKd>().def = true;
                
                Tasklar[sayac2] = TaskClone;
               
                TaskClone.GetComponent<TaskKd>().EntegreEdilenNesne = ModelParcalar[sayac2];
                TaskClone.transform.parent = GameObject.Find("Tasklar" + sayac).transform;

                Tasklar[sayac2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sayac + "." + (a + 1) + " " + TaskIsim[sayac2];
                TaskClone.transform.localScale = new Vector3(1, 1, 1);
                TaskClone.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -45 * (a + 1), 0);

                sayac2++;

                if(sayac2==Tasklar.Length)
                {
                    kayarmenud.GetComponent<KayarMenu>().PositionSonY = Tasklar[sayac2 - 1].GetComponent<RectTransform>().anchoredPosition.y;
                    
                }
            }
        }

        GameObject.Find(model_name).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(model_name + "/Animasyon/" + model_name) as RuntimeAnimatorController;

    }



    Ray ray;
    RaycastHit hit;
    public void Update()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit))
        {
            

            if (hit.collider.name != "ArkaPlan (1)")
            {
                if (Input.GetMouseButton(0))
                {
                    ModelTum.transform.Rotate((Input.GetAxis("Mouse Y") * ModelDondurmeHizi * Time.deltaTime), (-Input.GetAxis("Mouse X") * ModelDondurmeHizi * Time.deltaTime), 0, Space.World);
                }
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && scrool<2)
        {
            scrool += ModelYakinlastirmaHizi;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && scrool > 1)
        {
            scrool -= ModelYakinlastirmaHizi;
        }

        ModelTum.transform.localScale = new Vector3(scrool, scrool, scrool);




        if(devamedenanimasyonsayisi==AnimasyonSayisi)
        {
            TamAnBt.GetComponent<Button>().interactable = true;
        }
        else
        {
            TamAnBt.GetComponent<Button>().interactable = false;
        }
    }

    /*public void TamAnimasyonCalis()
    {
        TamAnimasyonAnimator.enabled = true;

        TamAnimasyonAnimator.Play(TamAnimasyonIsim);
    }*/



}
class TaskINPUT
{
    public string step_name { get; set; }
    //public string order_number { get; set; }
    //public string animation_rename { get; set; }

    public string piece_name { get; set; }
    public string model_name { get; set; }
    public string start { get; set; }
    public string finish { get; set; }

    public TaskINPUT(string step_name , string pieceName , string model_name , string start , string finish)
    {
        this.step_name = step_name;
        this.piece_name = pieceName;
        this.model_name = model_name;
        this.start = start;
        this.finish = finish;
    }
}

class StepINPUT
{
    public string order_number { get; set; }
    public string step_name { get; set; }
    public List<TaskINPUT> task_list { get; set; }
    public StepINPUT(string order_number , string step_name , List<TaskINPUT> task_list)
    {
        this.order_number = order_number;
        this.step_name = step_name;
        this.task_list = task_list;
    }
}



