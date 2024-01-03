using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralBonus : PowerUp
{
    public Ship recipient;
    public int amount;

    // Start is called before the first frame update
    public override void EffectStart()
    {
        base.EffectStart();
        if (recipient != null)
        {
            recipient.AddMineral(amount);
            DynamicTextManager.CreateText2D(Camera.main.ScreenToWorldPoint(transform.position), "+" + amount,DynamicTextManager.defaultData );
        }
            
    }

    public void Init(Vector2 position,Ship recipent)
    {
        transform.position = position;
        this.recipient = recipent;
    }
}
