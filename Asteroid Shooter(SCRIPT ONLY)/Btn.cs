
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum InputType
{
    HOLD,PRESS
}

public class Btn : MonoBehaviour
{
    public UnityEngine.Events.UnityAction action;
    public bool logicSignal;
    public InputType inputType;

    public TextMeshProUGUI text;
    
    protected virtual void Start()
        {
        if (transform.childCount>0)
                text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        public virtual void True()
        {
            logicSignal = true;
        }

        public virtual void False()
        {
            logicSignal = false;
        }
       
        
        protected virtual void Update()
        {   

            if (logicSignal && inputType!=InputType.PRESS)
                action();
        }
}
