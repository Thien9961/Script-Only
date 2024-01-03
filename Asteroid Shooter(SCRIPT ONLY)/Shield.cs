using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public void DestroyShield()
    {
        Destroy(gameObject);
    }

    public void TakeDamage()
    {
        GetComponent<Animator>().SetTrigger("takeDmg");
    }
}
