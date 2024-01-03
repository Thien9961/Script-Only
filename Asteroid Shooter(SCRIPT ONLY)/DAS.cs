using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DAS : MonoBehaviour
{
    public Image vfx;
    public AudioSource speaker;
    public float hpThreshold;//percent 

    void Update()
    {
        if (TryGetComponent(out Ship s))
        {
            if (s.hp < hpThreshold*s.maxHp && !s.invulnerable)
            {
                vfx.gameObject.SetActive(true);
                if (speaker != null)
                    speaker.enabled=true;
            }   
            else
            {
                if (speaker != null)
                    speaker.enabled = false;
                vfx.gameObject.SetActive(false);
            }
                
        }
        

    }
}