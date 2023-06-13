using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject TutorialCompletedScreen;
    [SerializeField] private TargetsManagerNetWork targetsManager;

    private bool isTutorialCompleted = false;
    private const string tutorialSceneName = "TutorialMode";
    private float nextLvlDely = 0.75f;


    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == tutorialSceneName)
            isTutorialCompleted = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //destroy the word and do sound and expl effects.
   void CompleteWord()
    {
        if (targetsManager.Count <= 0)
        {
            Debug.Log("fisnished");
            Invoke("GoToNextLvl", nextLvlDely);
        }
    }

    //move to the next lvl
    public void GoToNextLvl()

    {
       
        if (!isTutorialCompleted)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }

        TutorialCompletedScreen.SetActive(true);
        
    }
 
   
}
