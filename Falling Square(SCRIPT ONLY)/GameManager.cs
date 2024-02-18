using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Begin : State,IStateLoader
{
    GameObject startScreen;
    public override Context context { get; protected set; }

    public void Load()
    {
        
    }

    public void Unload()
    {
        startScreen.transform.position = GameObject.Find("Canvas").GetComponent<Canvas>().pixelRect.center;
        startScreen.gameObject.SetActive(false);
    }
    public override void Behavior()
    {
        GameManager.instance.score = 0;
        startScreen.gameObject.SetActive(true);
    }

    public Begin(Context context, GameObject startScreen) : base(context)
    {
        this.startScreen = startScreen;
    }
}

public class Playing : State,IStateLoader
{
    TextMeshProUGUI updateScore;
    public override Context context { get; protected set; }

    public void Load()
    {
        GameManager.instance.gameContext.state = GameManager.instance.playing;
    }

    public void Unload()
    {
        GameManager.instance.playScreen.transform.position = GameObject.Find("Canvas").GetComponent<Canvas>().pixelRect.center;
        GameManager.instance.playScreen.gameObject.SetActive(false);
    }
    public override void Behavior()
    {
        GameManager.instance.score = 0;
        UpdateScore(false);
        GameManager.instance.startScreen.SetActive(false);
        GameManager.instance.endScreen.SetActive(false);
        updateScore.gameObject.SetActive(true);
        GameManager.instance.listener.ForEach(x => x.GameStart());
    }

    public void UpdateScore(bool useEffect)
    {
        if(useEffect)
            updateScore.transform.DOPunchScale(Vector3.one * 1.1f, 0.25f);
        updateScore.text = GameManager.instance.score.ToString();
        
    }

    public Playing(Context context, TextMeshProUGUI updateScore) : base(context)
    {
        this.updateScore = updateScore;
    }
}

public class End : State,IStateLoader
{
    TextMeshProUGUI highscoreTxt,scoreTxt;
    GameObject endScreen;
    public override Context context { get; protected set; }

    public void Load()
    {
        
    }

    public void Unload()
    {
        endScreen.transform.position = GameObject.Find("Canvas").GetComponent<Canvas>().pixelRect.center;
        endScreen.SetActive(false);
    }
    public override void Behavior()
    {
        scoreTxt.text = GameManager.instance.score.ToString();  
        GameManager.instance.listener.ForEach(x => x.GameEnd());
    }

    public void ShowEndScreen() //Save highscore, play Sfx
    {
        GameManager.instance.loader.UnloadState(GameManager.instance.playing, GameManager.instance.playScreen.transform,Rail.instance.transform);
        GameManager.instance.loader.LoadState(GameManager.instance.over, GameManager.instance.endScreen.transform);
        if (PlayerPrefs.GetInt("high") < GameManager.instance.score)
        {
            highscoreTxt.text = GameMsg.NEW_BEST + GameManager.instance.score.ToString();
            SoundManager.instance.Play(SoundManager.instance.newHighScore);
            PlayerPrefs.SetInt("high", GameManager.instance.score);
            PlayerPrefs.Save();
            highscoreTxt.GetComponent<Animator>().Play("Flash");
        }
        else
        {
            highscoreTxt.GetComponent<Animator>().Play("Standby");
            highscoreTxt.text = GameMsg.BEST + PlayerPrefs.GetInt("high").ToString();
        }
            
        
    }
    
    public End(Context context,GameObject endScreen,TextMeshProUGUI highscore,TextMeshProUGUI score) : base(context)
    {
        highscoreTxt = highscore;
        scoreTxt=score;
        this.endScreen = endScreen;
    }
}

public class GameManager : Singleton<GameManager>
{
    public Context gameContext;
    public Button start,restart;
    public GameObject startScreen, endScreen, playScreen;
    public TextMeshProUGUI scoreTxt, highscoreTxt;
    public Begin begin;
    public Playing playing;
    public End over;
    public int score;
    public float delay=2;
    public StateLoader loader;


    public List<IListener> listener=new List<IListener>();
    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        gameContext = new Context();
        begin = new Begin(gameContext,startScreen);
        playing = new Playing(gameContext, playScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        over=new End(gameContext,endScreen,highscoreTxt,scoreTxt);
        gameContext.state = begin;
        loader = new StateLoader(gameContext, 0.5f);
        start.onClick.AddListener(() => { /*gameContext.state = playing;*/ SoundManager.instance.Play(SoundManager.instance.buttonClick);loader.LoadState(playing, playScreen.transform, Instantiate(Resources.Load<GameObject>("Rail").transform)); loader.UnloadState(begin, startScreen.transform);  });
        restart.onClick.AddListener(() => {  SoundManager.instance.Play(SoundManager.instance.buttonClick); loader.LoadState(playing, playScreen.transform, Instantiate(Resources.Load<GameObject>("Rail").transform)); loader.UnloadState(over, endScreen.transform); });
    }

}
