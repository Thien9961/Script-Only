using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static Ship player;
    public static int mineral,level,extraMineral,highscore;
    public Shop shop;
    public Enviroment[] enviroment;
    public static EdgeCollider2D screenBound;
    public static GameManager manager;
    public static float env_interval_coefficent;
    public static MusicPlayer musicManager;
    public float lvlupInterval, env_interval_descrease;//percent decrease
    public bool devMode,activated=false;//no saving progess

    public static Vector2 RandomLocInRect(Rect r) 
    {
        float x=Random.Range(r.xMin,r.xMax);
        float y=Random.Range(r.yMin,r.yMax);
        return new Vector2(x,y);
    }

    public void SpawnEnable(bool enable)
    {
        foreach(Enviroment e in enviroment)
                e.gameObject.SetActive(enable);
    }

    public void LevelUp()
    {
        if (env_interval_coefficent > 0)
        {
            env_interval_coefficent = 1 - Mathf.Clamp01(env_interval_descrease) * level;
            level++;
        }
        else
            CancelInvoke(nameof(LevelUp));
 
    }

    public static void Begin()
    {
        Reset();
        if (manager.lvlupInterval > 0)
            manager.InvokeRepeating(nameof(LevelUp), 0, manager.lvlupInterval);
        player = Instantiate(manager.shop.ship[Shop.selected], GameObject.Find("Canvas").transform).GetComponent<Ship>();
        manager.SpawnEnable(true);
    }

    public static void Reset()
    {
       Enviroment.ClearAll();
       manager.CancelInvoke(nameof(LevelUp));
       env_interval_coefficent = 1;
       level = 0;
       extraMineral = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        manager=GetComponent<GameManager>();
        musicManager= GameObject.Find("Music Manager").GetComponent<MusicPlayer>();
        musicManager.PlayAlbum("In Menu");
        Load();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            Save();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
            Save();
    }
    // Update is called once per frame
    public static void Save()
    {
        if(!manager.devMode)
        {
            Slider s1 = (Slider)UIManager.main.hashtable["Volume"], s2= (Slider)UIManager.main.hashtable["Joystick Sensitivity"];
            Toggle t = (Toggle)UIManager.main.hashtable["Viberation"];
            SaveGame.Save("vibration", t.isOn);
            SaveGame.Save("voloume", s1.value);
            SaveGame.Save("sensitivity", s2.value);
            SaveGame.Save("mineral", mineral);
            SaveGame.Save("selected", Shop.selected);
            foreach (Ship s in manager.shop.ship)
                SaveGame.Save(s.name, s.owned);
            Debug.Log("Saved");
        }   
    }

    public static void Load()
    {
        manager.activated = SaveGame.Load<bool>(nameof(activated));
        if(!manager.activated)
        {
            manager.activated = true;
            SaveGame.Save(nameof(activated), manager.activated);
            ResetProgess();
            Debug.Log("First Run");
        }
        else
        {
            Slider s1 = (Slider)UIManager.main.hashtable["Volume"], s2 = (Slider)UIManager.main.hashtable["Joystick Sensitivity"];
            Toggle t = (Toggle)UIManager.main.hashtable["Viberation"];
            t.isOn = SaveGame.Load<bool>("vibration");
            s1.value = SaveGame.Load<float>("voloume");
            s2.value = SaveGame.Load<float>("sensitivity");
            mineral = SaveGame.Load<int>("mineral");
            highscore = SaveGame.Load<int>("high");
            Shop.selected = SaveGame.Load<int>("selected");
            foreach (Ship s in manager.shop.ship)
                s.owned = SaveGame.Load<bool>(s.name);
            Debug.Log("Loaded");
        }   
    }
    
    public static void ResetProgess()
    {
        Slider s1 = (Slider)UIManager.main.hashtable["Volume"], s2 = (Slider)UIManager.main.hashtable["Joystick Sensitivity"];
        Toggle t = (Toggle)UIManager.main.hashtable["Viberation"];
        SaveGame.Save("vibration", true);
        SaveGame.Save("voloume", s1.maxValue);
        SaveGame.Save("sensitivity", s2.maxValue*0.2f);
        SaveGame.Save("mineral", manager.shop.ship[0].price);
        SaveGame.Save("selected", 0);
        SaveGame.Save("high", 0);
        foreach (Ship s in manager.shop.ship)
            SaveGame.Save(s.name, false);
        t.isOn = SaveGame.Load<bool>("vibration");
        s1.value = SaveGame.Load<float>("voloume");
        s2.value = SaveGame.Load<float>("sensitivity");
        mineral = SaveGame.Load<int>("mineral");
        Shop.selected = SaveGame.Load<int>("selected");
        foreach (Ship s in manager.shop.ship)
            s.owned = SaveGame.Load<bool>(s.name);
        Debug.Log("Reseted");
    }
}
