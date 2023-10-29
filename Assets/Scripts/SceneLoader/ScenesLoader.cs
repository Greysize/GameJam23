using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour
{
    Scene thisScene;
    // Start is called before the first frame update
    void Start()
    {
        thisScene = SceneManager.GetActiveScene();
        print ("SCENE NAME : " + thisScene.name);
        SceneManager.LoadScene("Level1_part1_Jeremi", LoadSceneMode.Additive);
        SceneManager.LoadScene("Level1_part2_Pierre", LoadSceneMode.Additive);
        SceneManager.LoadScene("Level1_part3_Nico", LoadSceneMode.Additive);
        SceneManager.SetActiveScene(thisScene);
        StartCoroutine(ForceActiveScene());
    }

    private IEnumerator ForceActiveScene()
    {
        yield return new WaitForSeconds(0.3f);
        print("force activeScene");
        SceneManager.SetActiveScene(thisScene);
        yield return null;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.SetActiveScene (thisScene);
        }
    }
}
