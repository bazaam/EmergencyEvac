using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = new LevelManager();
	void Start()
    {
		
	}
	



	void Update()
    {
		
	}


    public void NextLevel()
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        int i = CurrentScene.buildIndex;
        SceneManager.LoadScene(i + 1);
    }

}
