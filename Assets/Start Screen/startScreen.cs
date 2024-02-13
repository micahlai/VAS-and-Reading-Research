using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startScreen : MonoBehaviour
{
    public GameObject loginUI;
    public GameObject sceneUI;
    // Start is called before the first frame update
    void Start()
    {
        loginUI.SetActive(true);
        sceneUI.SetActive(false);
    }

    
    public void LoadScene(int s)
    {
        SceneManager.LoadScene(s);
    }
    public void showScenes()
    {
        loginUI.SetActive(false);
        sceneUI.SetActive(true);
    }
}
