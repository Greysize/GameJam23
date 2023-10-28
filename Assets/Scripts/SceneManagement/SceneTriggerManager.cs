using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;


//[RequireComponent(typeof(BoxCollider))]
public class SceneTriggerManager : MonoBehaviour
{
    [Header("Scenes to Load")]
    public SceneFieldAsset[] m_scenesToLoad;

    [Header("Alternative Scenes")]
    public SceneFieldAsset[] m_previousScenes;

    [Header("Scene to Unload")]
    [SerializeField] SceneFieldAsset[] m_scenesToUnload;

    [Header("Scene Setup")]
    [SerializeField] int sceneIndexToBeActive;
    [SerializeField] bool activeSceneManually;

    [Header("Events")]
    public float SceneChangeWait;
    public UnityEvent OnSceneChange;
    public UnityEvent OnSceneChangedFinished;
    private SceneFieldAsset[] restoreScene;
    private string restoreSpawn;

    //[Header("Prevent Trigger exessive loading")]
    //[SerializeField] float timeToActivateCollider;
    //private BoxCollider thisBoxCollider;

    private void Start()
    {
        //AssignBoxCollider();
        //StartCoroutine(ActivateCollider());

        if (activeSceneManually)
        {
            try
            {
                print(SceneManager.GetSceneByName(m_scenesToLoad[sceneIndexToBeActive].SceneName));
            }

            catch (Exception e)
            {
                print(e);
                Debug.LogError("Please, select an index from: " + 0 + " to " + (m_scenesToLoad.Length - 1) + " on scenesToLoad array");
            }
        }
    }


    public void TriggerChangeScenes()
    {
        StartCoroutine(ChangeSceneCoRoutine());
    }

    IEnumerator ChangeSceneCoRoutine()
    {
        OnSceneChange.Invoke();
        yield return new WaitForSeconds(SceneChangeWait);
        print(" >> Changing scene = " + m_scenesToLoad[0].m_sceneName);
        if (activeSceneManually)
            SceneManagement.singleton.SceneLoadCallback = SetActiveSceneManually;

        SceneManagement.singleton.LoadScenes(m_scenesToLoad);
        SceneManagement.singleton.SetScenesToUnload(m_scenesToUnload);
        OnSceneChangedFinished.Invoke();
    }

    public void HardLoad()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(m_scenesToLoad[0], LoadSceneMode.Single);
    }

    void SetActiveSceneManually()
    {
        var sceneExists = m_scenesToLoad[sceneIndexToBeActive] != null;

        if (sceneExists)
            SceneManagement.singleton.SetActiveSceneManually(SceneManager.GetSceneByName(m_scenesToLoad[sceneIndexToBeActive].SceneName));
    }

    public void ChangeSceneToLoadByAsset(SceneFieldAsset SceneInfo)
    {
        if (SceneInfo != null)
        {
            // reset and refill the scene to load
            m_scenesToLoad = new SceneFieldAsset[1];
            m_scenesToLoad[0] = SceneInfo;
        }

    }
    public void ChangeSceneToLoadByName(string sceneName)
    {
        if (sceneName != null)
        {
            var temp = new SceneFieldAsset();
            temp.m_sceneName = sceneName;
            SceneFieldAsset[] tempArr = new SceneFieldAsset[1];
            tempArr[0] = temp;
            print(" Scene : " + tempArr[0].m_sceneName);
            m_scenesToLoad = tempArr;
        }
    }

    public void ChangeSceneToUnloadByName(string sceneName)
    {
        if (sceneName != null)
        {
            var temp = new SceneFieldAsset();
            temp.m_sceneName = sceneName;
            SceneFieldAsset[] tempArr = new SceneFieldAsset[m_scenesToUnload.Length + 1];
            for (int i = 0; i < m_scenesToUnload.Length; i++)
            {
                tempArr[i] = m_scenesToUnload[i];
            }
            tempArr[m_scenesToUnload.Length] = temp;
            print(" Scene : " + tempArr[0].m_sceneName);
            m_scenesToUnload = tempArr;
        }
    }

    public bool SwapToPreviousScene()
    {
        bool result = false;
        if (m_previousScenes.Length != 0)
        {
            restoreScene = m_scenesToLoad;
            m_scenesToLoad = m_previousScenes;
            m_previousScenes = restoreScene;
            print(" new next scene to load is : " + m_scenesToLoad[0].m_sceneName);
            result = true;
        }
        else
        {
            Debug.Log(" > No previous scene");
        }
        return result;
    }

}

