using System.Collections;
using UnityEngine;
using System;
using DG.Tweening;

public interface IListener
{
    public void GameStart();
    public void GameEnd();
}

public interface IStateLoader
{
    public void Load();
    public void Unload();
}

public struct GameMsg
{
    public static readonly string NEW_BEST = "NEW BEST ",
                                  BEST = "BEST ";
}

public abstract class State
{
    public abstract Context context { get; protected set; }
    public abstract void Behavior();
    public State(Context context)
    {
        this.context = context;
    }
}

public sealed class Context
{
    State _state;
    public State state { get { return _state; } set { SetState(value); } }
    public void SetState(State state)
    {
        if (state.context == this)
        {
            _state = state;
            state.Behavior();
        }
    }
}

public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T instance;

    protected virtual void Awake()
    {
        if (instance == null)
            instance = this as T;
        else
            DestroyImmediate(this);
    }
}


public interface IDestructible
{
    public Color faction { get; set; }
    public void Birth();
    public void Death();
}

public struct Faction
{
    public static readonly Color CIRCLE = new Color(0.902f, 0.275f, 0.294f, 1),
                                 SQUARE = new Color(0.208f,0.208f,0.255f,1);
}

public static class Extension
{
    public static void WaitForSeconds(this MonoBehaviour mono, float seconds, Action action)
    {
        mono.StartCoroutine(IWaitForSeconds(seconds, action));
    }
    static IEnumerator IWaitForSeconds(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}

public class StateLoader
{
    public Context context;
    public float loadTime = 1f;

    public StateLoader(Context context,float loadTime)
    {
        this.loadTime = loadTime;
    }

    public void LoadState(IStateLoader state, params Transform[] component)
    {
        float screenDistance =  GameObject.Find("Canvas").GetComponent<Canvas>().pixelRect.height;
        foreach(Transform t in component)
        {
            if(!t.TryGetComponent(out Rail r))
            {
                t.position = new Vector3(t.position.x, t.position.y - screenDistance, 0);
                t.gameObject.SetActive(true);
                t.DOMove(new Vector3(t.position.x, t.position.y + screenDistance, 0), loadTime);
            }
            else
            {
                Rail.instance = r;
                t.position = new Vector3(t.position.x, t.position.y - 12, 0);
                t.gameObject.SetActive(true);
                t.DOMove(new Vector3(t.position.x, t.position.y + 12, 0), loadTime);
            }
        }
        Extension.WaitForSeconds(GameManager.instance, loadTime, state.Load);
    }

    public void UnloadState(IStateLoader state, params Transform[] component)
    {
        float screenDistance =  GameObject.Find("Canvas").GetComponent<Canvas>().pixelRect.height;
        foreach (Transform t in component)
        {
            if (!t.TryGetComponent(out Rail r))
            {
                t.DOMove(new Vector3(t.position.x, t.position.y + screenDistance, 0), loadTime);
            }
            else
            {
                //t.DOMove(new Vector3(t.position.x, t.position.y + 12, 0), loadTime);
                //Extension.WaitForSeconds(Rail.instance, loadTime + 0.1f, t.GetComponent<Rail>().Unload);
                Rail.instance.Unload();
            }
            Extension.WaitForSeconds(GameManager.instance, loadTime+0.1f, state.Unload);
        }


       
    }
}
