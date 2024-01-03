using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCallback : MonoBehaviour
{
    public Shop shop;
    public UIManager manager;
    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Unhide()
    {
        gameObject.SetActive(true);
    }
    public virtual void Birth()
    {
        
    }

    public void Reward()
    {
        Hide();
        GameManager.mineral += GameManager.extraMineral;
        GameManager.manager.shop.Preset();
        GameManager.Save();
        GameManager.extraMineral = 0;
    }

    private void OnParticleSystemStopped()
    {
        if(name=="Explosion(Clone)")
            Enviroment.pool.TakeToPool(12, transform);
        else
            Enviroment.pool.TakeToPool(13, transform);
    }

    public void ShopIO()
    {
        shop.Display(!shop.gameObject.activeSelf);
        manager.gameObject.SetActive(!manager.gameObject.activeSelf);
        if (shop.gameObject.activeSelf)
        {
            manager.SetBackground("shop_bg", Color.white);
            GameManager.musicManager.PlayAlbum("In Shop");
        }
        else
        {
            manager.gameObject.SetActive(true);
            GameManager.musicManager.PlayAlbum("In Menu");
            manager.SetBackground("menu_bg", Color.white);
        }

    }


}
