using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HUDExtension : MonoBehaviour
{
    //private void OnEnable()
    //{
    //    SceneManager.activeSceneChanged += HandleSceneChanged;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.activeSceneChanged -= HandleSceneChanged;
    //}

    //private void HandleSceneChanged(Scene arg0,Scene arg1)
    //{
    //    GetComponent<NetworkManagerHUD>().
    //    //GetComponent<NetworkManagerHUD>().showGUI = (arg1.name == "Menu" )? false : true;
    //}

    private void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);
        if(scene.name=="Menu")
        {
            GetComponent<NetworkManagerHUD>().enabled = false;
        }
        else
        {
            GetComponent<NetworkManagerHUD>().enabled = true;
        }
    }

}
