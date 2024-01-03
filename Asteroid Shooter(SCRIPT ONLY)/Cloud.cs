using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : FlyingObject
{
    public DustCloud dustCloud;
    public override void Death(GameObject killer)
    {
        base.Death(killer);        
        dustCloud.StartCoroutine(dustCloud.NaturalSpawn());  
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (transform.localScale.x > 0.9 * maxSize && !DOTween.IsTweening(transform))
            GetComponent<SpriteRenderer>().DOColor(new Color(255, 255, 255, 0), 3); 
    }
}
