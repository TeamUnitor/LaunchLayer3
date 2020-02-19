using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveButton : MonoBehaviour
{
    public static string SceneName = "MainMenu";

    public void CancelButton() {
        SceneManager.LoadScene(SceneName);
    }

    public void Save_XML() {
        
    }
}
