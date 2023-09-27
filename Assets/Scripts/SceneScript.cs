using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine.UI;

public class SceneScript : NetworkBehaviour
{
    public Text canvasBUlletText;
    public Text canvasStatusText;
    public PlayerScipts playerScript;

    [SyncVar(hook =nameof(OnStatusTextChanged))]
    public string statusText;

    private void OnStatusTextChanged(string oldStr,string newStr)
    {
        canvasStatusText.text = newStr;
    }

    public void ButtonSendMessage()
    {
        if(playerScript!=null)
        {
            playerScript.CmdSendPlayerMessage();
        }
    }

    public void ButtonChangeScene()
    {
        if(isServer)
        {
            var scene = SceneManager.GetActiveScene();      //获取当前场景    
            NetworkManager.singleton.ServerChangeScene(scene.name == "MyScene" ? "MyOtherScene" : "MyScene");
        }
        else
        {
            Debug.Log("You aren`t server!!!");
        }
    }
}
