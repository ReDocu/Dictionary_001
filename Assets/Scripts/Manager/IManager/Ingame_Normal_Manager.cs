using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ingame_Normal_Manager : IManager
{
    public override void Enter()
    { 
        testElaspedTime = 0;
        Debug.Log("Ingame Normal Manager 진입");
        
        var task =  SceneManager.LoadSceneAsync(sceneIndex).ToUniTask();
        AwaitCompleteLoad(task).Forget();
    }
    public override void Realease()
    {
        Debug.Log("Ingame Normal Manager 해제");
        isLoadComplete = false;
    }
    public override void Update()
    {
        if(!isLoadComplete)
            return;
        
        testElaspedTime += Time.deltaTime;
        if (testElaspedTime >= testSceneChangeTime)
        {
            ChangeState<Ingame_Boss_Manager>();
        }
    }
}
