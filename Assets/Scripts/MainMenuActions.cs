using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        #if (UNITY_EDITOR)
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        #if (UNITY_STANDALONE || UNITY_STANDALONE_LINUX)
            Application.Quit();
        #endif
    }
}
