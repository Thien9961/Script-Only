using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class Protection : PowerUp
{
    public float duration;
    public Ship target;
    public Shield shield;


    public void Init(Vector2 position, Ship target)
    {
        this.target = target;
        transform.position = position;
    }
    public override void EffectStart()
    {
        base.EffectStart();
        CancelInvoke(nameof(EffectEnd));
        if (target.shield!=null)
        {
            if (duration > 0)
                Invoke(nameof(EffectEnd), duration);
        }
        else
        {
            target.shield=Instantiate(shield, target.transform.Find("HUD").transform).GetComponent<Shield>();
            if (duration > 0)
                Invoke(nameof(EffectEnd), duration);
            target.invulnerable = true;
        }  
    }

    public void EffectEnd()
    {
        if(target.shield != null && target.HUD.transform.GetComponentInChildren<Shield>() != null)
        {
            target.invulnerable = false;
            target.shield.GetComponent<Animator>().SetBool("alive", false);
            target.shield = null;
        }        
    }

}
