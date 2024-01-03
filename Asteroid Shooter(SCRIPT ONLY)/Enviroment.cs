using System.Collections;
using UnityEngine;
using Redcode.Pools;
using System.Linq;
using Utility;
using UnityEngine.SceneManagement;

public class Enviroment : MonoBehaviour
{
    public static PoolManager pool;
    [HideInInspector]
    public Rect spawnArea;
    public Vector2 areaScale= Vector2.one;
    public float maxInterval, minInterval;
    public static readonly float spatialExtend=18f;
    //spatial_extend = spaceship's_speed*(desired_travel_time/fixed_delta_time)
    public static int layerOrder= 32767;

    // Start is called before the first frame update

    protected virtual void Start()
    {
        spawnArea = GameObject.Find("Canvas").GetComponent<Canvas>().pixelRect;
        Rec.Scale(ref spawnArea,areaScale.x,areaScale.y);
        pool = GameObject.Find("Pool Manager").GetComponent<PoolManager>();
        //Debug.Log("Env_layerOrder: " + layerOrder);
    }

    protected void OnEnable()
    {
        StartCoroutine(NaturalSpawn());
    }

    public static void ClearAll()
    {
        GameObject cleaner = new GameObject("Cleaner");
        cleaner.AddComponent<Enviroment>().StopCoroutine(nameof(NaturalSpawn));
        GameObject[] list = SceneManager.GetActiveScene().GetRootGameObjects();
        var v = from GameObject g in list
                where g.GetComponent<Enviroment>() != null
                select g;
        foreach (GameObject go in v)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform child = go.transform.GetChild(i);
                if (child.TryGetComponent(out FlyingObject f) && child.GetComponent<Enviroment>() == null)
                {
                    DG.Tweening.DOTween.Kill(child.transform);
                    Destroy(child.gameObject);
                }
                    
            }
        }
        Destroy(cleaner);
    }

    public virtual IEnumerator NaturalSpawn()
    {
        yield return null;
    }
}
