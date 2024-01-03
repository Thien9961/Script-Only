using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Shop : UIMenu
{
    public Ship[] ship;
    public UIMenu popup;
    public static int selected = 0;
    public void NextShip()
    {
        if (selected + 1 < ship.Length)
            selected++;
        else
            selected = 0;
        Preset();
        GameManager.Save();
    }

    public void PreviousShip()
    {
        if(selected-1 >= 0)
            selected--;
        else
            selected=ship.Length-1;
        Preset();
        GameManager.Save();
    }

    public override void SetButtonAction(string buttonName, UnityAction action)
    {
        Button b = (Button)hashtable[buttonName];
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(action);
        b.onClick.AddListener(playSfx);
    }

    public override void Awake()
    {
        base.Awake();
        popup.playSfx= playSfx;
        popup.Init();
        popup.SetButtonAction("Button", () => { popup.SetAnimatorBool("Pop-up", "active", false); });
        Preset();
    }

    public void ShowShipInfo()
    {
        Ship s = ship[selected];
        SetText("Ship Name Text", s.name);
        SetText("Mineral Text", "Mineral: " + GameManager.mineral.ToString());
        SetText("Ship Stat Text", s.GetInfo());
        SetImage("Ship Image", s.GetComponent<Image>());
    }

    public override void Preset()
    {
        Ship s = ship[selected];
        ShowShipInfo();
        SetButtonAction("Next Ship Button", NextShip);
        SetButtonAction("Previous Ship Button", PreviousShip);
        if (s.owned)
        {
            SetButtonText("Select Ship Button", "Play");
            SetButtonAction("Select Ship Button", GameManager.Begin);
            SetObjectActive("Lock Image", false);
        }   
        else
        {
            SetButtonText("Select Ship Button", "Unlock: " + s.price);
            SetButtonAction("Select Ship Button", Sell);
            SetObjectActive("Lock Image", true);
        }
            
    }

    

    public void Sell()
    {
            if (GameManager.mineral >= ship[selected].price)
            {
                GameManager.mineral -= ship[selected].price;
                ship[selected].owned =true;
                SetText("Mineral Text", "Mineral: "+GameManager.mineral.ToString());
                SetButtonAction("Select Ship Button", GameManager.Begin);
                SetButtonText("Select Ship Button", "Play");
                SetAnimatorBool("Lock Image","isLocked", false);
                GameManager.Save();
            }
            else
            {
                popup.SetText("Message", "Not enough mineral.");
                popup.Display(true);
            }
    }

}

