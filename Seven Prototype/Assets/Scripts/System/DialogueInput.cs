﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueInput : MonoBehaviour
{
    public UnityEvent OnRequestNextLine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDialogueAdvance(){
        OnRequestNextLine?.Invoke();
    }
}
