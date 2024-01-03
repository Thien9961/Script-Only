using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSpeed : PowerUp
{
    public float duration, speedGain;
    public bool invulnerable;
    public Ship user;

    public void Init(Vector2 position,Ship user)
    {
        this.user = user;
        transform.position= position;
    }
    public override void EffectStart()
    {
        base.EffectStart();
        if (duration > 0)
            Invoke(nameof(EffectEnd), duration);
        else
            EffectEnd();
        if (duration < Mathf.Infinity)
            Invoke(nameof(EffectEnd), duration);
        user.speed *= speedGain;
        user.invulnerable = invulnerable;

    }

    public void EffectEnd()
    {
        user.speed /= speedGain;
        if(invulnerable)
            user.invulnerable = !invulnerable;
    }

}
