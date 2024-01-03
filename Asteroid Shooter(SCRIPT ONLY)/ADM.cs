using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ADM : MonoBehaviour
{
    public GameObject indicator;
    public float sensivity=2.5f;
    public int indicatorLimit=999;
    private Hashtable memory=new Hashtable();
    public Asteroid[] data;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Hashtable Detect()
    {
        GameObject canvas = GameObject.Find("Canvas");
        foreach (Asteroid a in AsteroidField.GetActiveAsteroid())
        {
            bool  b, c, d, e;
            e = Mathf.Abs(a.transform.localScale.x) > sensivity;
            b = memory.Count < indicatorLimit;
            c = !memory.Contains(a);
            d = canvas.GetComponent<Canvas>().pixelRect.Contains(Camera.main.WorldToScreenPoint(a.transform.position));
            if ( b && c && d && e )
            {
                GameObject g = Instantiate(indicator,GameManager.player.transform);
                memory.Add(a, g);
                Image i = g.GetComponent<Image>();
                i.transform.position = Camera.main.WorldToScreenPoint(a.transform.position);
                i.raycastTarget = false;
            }
            else if (!c && !e)
            {
                GameObject g = (GameObject)memory[a];
                memory.Remove(a);
                Destroy(g);
            }
        }
                       
        return memory;
    }

    public void Trace()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (DictionaryEntry e in memory)
        {
            Asteroid asteroid = (Asteroid)e.Key;
            GameObject tracer = e.Value as GameObject;
            if (asteroid.gameObject.activeSelf)
                tracer.transform.position = Camera.main.WorldToScreenPoint(asteroid.transform.position);               
            else
            {
                list.Add(asteroid.gameObject);
                Destroy(tracer);                    
            }
        }
        foreach (var v in list)
            memory.Remove(v);
    }
    // Update is called once per frame

    void FixedUpdate()
    {
        Detect();
        Trace();
    }
}
