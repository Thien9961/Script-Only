
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(SliderJoint2D))]
public class Rail : Singleton<Rail>,IDestructible,IListener,IStateLoader
{
    public Color faction { get; set; }
    public KeyCode input = KeyCode.Space;
    public Animator animator;
    // Start is called before the first frame update

    public void Load()
    {
        
    }

    public void Unload()
    {
        GameManager.instance.listener.Remove(this);
        DestroyImmediate(gameObject);
        instance = null;
    }

    public void GameStart()
    {
        transform.GetChild(0).gameObject.SetActive(true);

    }

    public void GameEnd()
    {
        
    }
    public void SwtichDirection(bool auto)
    {
        if (TryGetComponent(out SliderJoint2D joint))
        {
            joint.angle +=-180;
        }
        if (auto)
            SoundManager.instance.Play(SoundManager.instance.autoSwitch);
        else if(GameManager.instance.gameContext.state==GameManager.instance.playing)
            SoundManager.instance.Play(SoundManager.instance.forceSwitch);
    }

    // Update is called once per frame
    void Update()
    {
        bool b = Input.GetKeyDown(input) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || (Input.touchCount==1 && Input.touches[0].phase== TouchPhase.Began);
        if (b)
            SwtichDirection(false);

    }

    private void OnEnable()
    {
        Birth();
    }

    public void Birth() { animator = GetComponent<Animator>(); GameManager.instance.listener.Add(instance); }
    public void Death() { }
}
