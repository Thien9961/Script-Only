using UnityEngine;
using UnityEngine.UI;

public class UIManager : UIMenu
{
    public Shop shop;
    public static UIManager main;
    public UIMenu setting, info, policy,eula,gameOver;
    public string appURL;


    public override void Preset()
    {
        //UnityAction<float> set_joystick_sens = (value) => { Weapon.sensivity = value; };
        SetBackground("menu_bg",Color.white);
        SetButtonAction("Play Button", ShopIO);
        SetButtonAction("Setting Button", SettingIO);
        SetButtonAction("Privacy Button", PrivacyIO);
        SetButtonAction("Rate Button", ToRating);
        SetButtonAction("Info Button", InfoIO);
        SetButtonAction("Exit Info", InfoIO);
        SetButtonAction("Exit Setting", SettingIO);
        //SetButtonAction("Exit Shop", ShopIO);
        SetButtonAction("Exit Privacy", PrivacyIO);
        SetButtonAction("Lisence Button", EULAIO);
        SetButtonAction("Exit EULA", EULAIO);
        //SetSliderAction("Joystick Sensitivity", set_joystick_sens);
    }

    public override void Awake()
    {
        base.Awake();
        Init();
        main = GetComponent<UIManager>();
        shop.Init();
        setting.Init();
        info.Init();
        gameOver.playSfx = playSfx;
        gameOver.Init();
        gameOver.SetButtonAction("Button", () => { gameOver.SetAnimatorBool("Game Over", "active", false); });
        gameOver.SetButtonAction("Double", () => { gameOver.SetAnimatorBool("Game Over", "active", false); /*GameObject.Find("Ads Manager").GetComponent<InterstitialAdExample>().ShowAd();*/ });
        gameOver.SetButtonAction("Back to menu", () => { gameOver.SetAnimatorBool("Game Over", "active", false); });
        Preset();
        Slider s = (Slider)hashtable["Volume"];
        s.onValueChanged.AddListener((val) => { AudioListener.volume = val; });

    }

    public void SetBackground(string spritePath,Color color)
    {
        Image background = (Image)hashtable["Background"];
        background.sprite = Resources.Load<Sprite>(spritePath);
        background.color=color;
        background.raycastTarget = false;
    }

    public void ShopIO()
    {  
       
    }

    public void SettingIO()
    {
        setting.gameObject.SetActive(!setting.gameObject.activeSelf);
        gameObject.SetActive(!gameObject.activeSelf);
        SetBackground("menu_bg", Color.white);
    }



    public void GameIO() 
    {
        GameManager.musicManager.PlayAlbum("In Game");
        shop.Display(false);
        SetBackground("shop_bg", Color.clear);
        gameObject.SetActive(false);
    }

    public void PrivacyIO()
    {
        policy.gameObject.SetActive(!policy.gameObject.activeSelf);
        gameObject.SetActive(!gameObject.activeSelf);
        SetBackground("menu_bg", Color.white);
    }

    public void EULAIO()
    {
        SetBackground("menu_bg", Color.white);
        eula.gameObject.SetActive(!eula.gameObject.activeSelf);
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void ToRating()
    {
        #if UNITY_ANDROID
            Application.OpenURL(string.Format("market://details?id=" + Application.identifier));
        #elif UNITY_IPHONE
            Application.OpenURL("itms-apps://itunes.apple.com/app/" + Application.identifier);
        #endif
    }

    public void InfoIO()
    {
        info.gameObject.SetActive(!info.gameObject.activeSelf);
        gameObject.SetActive(!gameObject.activeSelf);
        SetBackground("menu_bg", Color.white);
    }
}
