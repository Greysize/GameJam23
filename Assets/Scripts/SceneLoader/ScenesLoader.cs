using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("Level1_part1_Jeremi", LoadSceneMode.Additive);
        SceneManager.LoadScene("Level1_part2_Pierre", LoadSceneMode.Additive);
        SceneManager.LoadScene("Level1_part3_Nico", LoadSceneMode.Additive);

    }

}
