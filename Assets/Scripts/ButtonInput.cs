using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;




public class ButtonInput : Selectable
{

    private bool clicked = false;

    [Serializable]
    public class OnPressedEvent : UnityEvent { }

    [Serializable]
    public class OnReleasedEvent : UnityEvent { }

    [FormerlySerializedAs("onPressed")]
    [SerializeField]
    public OnPressedEvent onPressedButton = new OnPressedEvent();

    [FormerlySerializedAs("onReleased")]
    [SerializeField]
    public OnReleasedEvent onReleasedButton = new OnReleasedEvent();


    protected override void Awake()
    {

    }

    private void FixedUpdate()
    {
        
    }

    public void Checked()
    {
        if (base.IsPressed())
        {
            onPressedButton.Invoke();
            clicked = true;
        }
        else if(clicked = true && !base.IsPressed())
        {
            clicked = false;
            onReleasedButton.Invoke();
        }
    }

    



}
