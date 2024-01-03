using BayatGames.SaveGameFree;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCallback : EventCallback
{
    public List<Button> button;
    public UIMenu rewardNotice;
    public GameObject scoreboard;
    public TextMeshProUGUI gameResult;

    public string result;
    int _index;

    private void Awake()
    {
        button[0].onClick.AddListener(SetAction0);
        button[1].onClick.AddListener(SetAction1);
        button[2].onClick.AddListener(SetAction2);
        rewardNotice.Init();
        rewardNotice.SetButtonAction("OK", () => { rewardNotice.SetAnimatorBool("Reward Notice", "active", false); Reward(); }) ;
    }

    public void SetAction0()
    {
        _index = 0;
    }

    public void SetAction1()
    {
        _index = 1;
    }

    public void SetAction2()
    {
        _index = 2;
    }

    public void Action()
    {   
        switch (_index)
        {
            case 0:
                {
                    ToMenu();
                    break;
                }
            case 1:
                {
                    Restart();
                    break;
                }
            case 2:
                {
                    Double();
                    break;
                }


        }
    }
    void Restart()
    {
        Hide();
        ShopIO();
    }

    void Double()
    {
        rewardNotice.SetText("Message", "+" + GameManager.extraMineral);
        rewardNotice.gameObject.SetActive(true);
        Restart();
    }


    void ToMenu()
    {
        Hide();
        GameManager.musicManager.PlayAlbum("In Menu");
        UIManager.main.gameObject.SetActive(true);
        UIManager.main.SetBackground("menu_bg", Color.white);
    }
    // Create a generic delegate for the button click event.

    public void ShowBoard()
    {
        gameResult.text = result;
        scoreboard.SetActive(true);
    }

    public void SetResult(int collectedMineral,int repairCost, int ammCost)
    {
        int score = collectedMineral - ammCost - repairCost;
        if (GameManager.highscore < score)
            SaveGame.Save("high", score);
        GameManager.mineral += score;
        GameManager.Save();
        GameManager.extraMineral = score;
        result = $"<color=yellow> {SaveGame.Load<int>("high")}</color>\n\n<color=green>+{collectedMineral}</color>\n<color=red>-{repairCost}</color>\n<color=red>-{ammCost}</color>\n\n{score}";
    }
}

