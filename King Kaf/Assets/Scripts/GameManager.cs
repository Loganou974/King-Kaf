using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : Manager
{
    //first Generate Level via chunkGeneration
    //then Generate perso

    public Manager[] scriptsToInit;
  

    private void Awake()
    {
        InitAll();

    }

    private void Start()
    {
        
    }

    private void InitAll()
    {
        for (int i = 0; i < scriptsToInit.Length; i++)
        {
            scriptsToInit[i].Init();
            Debug.Log(scriptsToInit[i].gameObject.name + " initialized");
        }
    }
}
