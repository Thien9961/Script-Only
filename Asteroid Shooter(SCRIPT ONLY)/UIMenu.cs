using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    public Hashtable hashtable = new Hashtable();
    public TextMeshProUGUI[] text;
    public Button[] button;
    public Slider[] slider;
    public Toggle[] toggle;
    public Dropdown[] dropdown;
    public Image[] image;
    public AudioClip clickSfx;

    public UnityAction playSfx;


    /// <summary>
    /// Initialize the menu, have to call this method manually at choosen time and before calling any method below.
    /// </summary>

    public virtual void Init()
    {
        foreach (var item in text)
            hashtable.Add(item.name, item);
        foreach (var item in button)
            hashtable.Add(item.name, item);
        foreach (var item in slider)
            hashtable.Add(item.name, item);
        foreach (var item in toggle)
            hashtable.Add(item.name, item);
        foreach (var item in dropdown)
            hashtable.Add(item.name, item);
        foreach (var item in image)
            hashtable.Add(item.name, item);
    }

    public virtual void Preset()
    {

    }

    public void OnEnable()
    {
        if (TryGetComponent(out AudioSource s))
            s.Play();
    }

    public virtual void Awake()
    {
        playSfx = () => { AudioSource.PlayClipAtPoint(clickSfx, Camera.main.transform.position); }; 
    }

    public virtual void Display(bool display)
    {
        Preset();
        gameObject.SetActive(display);
    }

    public void SetObjectActive(string objectName, bool active)
    {
        var v = hashtable[objectName] as Component;
        v.gameObject.SetActive(active);
    }

    public void SetText(string textName, string message)
    {
        TextMeshProUGUI t = (TextMeshProUGUI)hashtable[textName];
        t.SetText(message);
    }

    public void SetImageColor(string imageName, Color color) 
    {
        Image i = (Image)hashtable[imageName];
        i.color = color;
    }

    public void SetAnimatorBool(string objectName,string paramName, bool b)
    {
        Image i = (Image)hashtable[objectName];
        i.GetComponent<Animator>().SetBool(paramName, b);
    }

    public void SetButtonText(string buttonName, string message)
    {
        Button b = (Button)hashtable[buttonName];  
        if (b.transform.childCount > 0)
            b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
    }

    public virtual void SetButtonAction(string buttonName, UnityAction action)
    {
        Button b = (Button)hashtable[buttonName];
        b.onClick.AddListener(action);
        b.onClick.AddListener(playSfx);
    }

    public void SetImage(string imageName, Image image)
    {
        Image i = (Image)hashtable[imageName];
        i.sprite = image.sprite;
    }

    public void SetSprite(string imageName, Sprite sprite)
    {
        Image i = (Image)hashtable[imageName];
        i.sprite = sprite;
    }

    public virtual void SetSliderAction(string sliderName, UnityEngine.Events.UnityAction<float> action)
    {
        Slider s = (Slider)hashtable[sliderName];
        s.onValueChanged.RemoveAllListeners();
        s.onValueChanged.AddListener(action);
    }
}

