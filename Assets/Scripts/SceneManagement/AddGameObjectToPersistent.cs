using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AddGameObjectToPersistent : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {
            SceneManagement SceneManager = FindObjectOfType<SceneManagement>();
            if(SceneManager != null)
            {
                var temp = SceneManager.objectsToMoveToActiveScene;
                GameObject[] temp2 = new GameObject[temp.Length + 1];
                int i = 0;
                foreach (GameObject node in temp)
                {
                    temp2[i] = temp[i];
                    i++;
                }
                temp2[i] = gameObject;
                SceneManager.objectsToMoveToActiveScene = temp2;
                // delete the componenet
                Destroy(gameObject.GetComponent<AddGameObjectToPersistent>());
            }
        }

    }
