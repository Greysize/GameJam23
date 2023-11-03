using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class SceneManagement : MonoBehaviour
{
    public Action SceneLoadCallback;
    public static SceneManagement singleton = null;
    private Scene activeScene;
    private List<SceneFieldAsset> currentScenesToUnload;
    private int countCurrentLoadedScenes;
    private int countScenesToLoad;
    [Header("Set last loaded scene as active")]
    [Tooltip("Should last loaded scene be active?")]
    [SerializeField] public bool lastLoadedSceneIsActive;

    [Header("Persistent Objects")]
    [Tooltip("These objects will be moved to the current active scene")]
    public GameObject[] objectsToMoveToActiveScene;
    List<string> tempLoadedScenes = new List<string>();

    [Header("Player")]
    public GameObject PlayerGo;


    [Header("VR Options")]
    //public ScreenFade ScreenFader;
    public GameObject SphereFader;

    private Animator FaderAnim;

    private bool isprocessing = false;

    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        SceneManager.sceneUnloaded += SceneUnloaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        SceneManager.activeSceneChanged -= ActiveSceneChanged;
        SceneManager.sceneUnloaded -= SceneUnloaded;
    }

    private void Awake()
    {
        //SphereFader = FindObjectOfType<FaderItem>().gameObject;
        //FaderAnim = SphereFader.GetComponent<Animator>();
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        else if (singleton != this)
        {
            DestroyImmediate(gameObject);
        }
    }

    void SetLastLoadedSceneActiveScene()
    {
        SceneManager.SetActiveScene(activeScene);
    }

    public void SetActiveSceneManually(Scene activeScene)
    {
        lastLoadedSceneIsActive = false;
        SceneManager.SetActiveScene(activeScene);
        this.activeScene = activeScene;
    }

    void UnloadScenes()
    {
        countCurrentLoadedScenes = 0;

        foreach (var scene in currentScenesToUnload)
        {
            var alreadyLoaded = SceneManager.GetSceneByName(scene.SceneName).isLoaded;

            if (alreadyLoaded)
                SceneManager.UnloadSceneAsync(scene);
        }
        //StartCoroutine(CoFader(false));
    }

    void ClearTempScenes()
    {
        if (tempLoadedScenes.Count > 0)
            tempLoadedScenes.Clear();
    }

    public void LoadScenes(SceneFieldAsset[] scenesToLoad)
    {
        countScenesToLoad = scenesToLoad.Length;

        foreach (var scene in scenesToLoad)
        {
            var alreadyLoaded = SceneManager.GetSceneByName(scene.SceneName).isLoaded;

            if (!alreadyLoaded)
                StartCoroutine(LoadSceneAdditive(scene));
        }
    }

    void MoveObjectsToActiveScene()
    {
        foreach (var go in objectsToMoveToActiveScene)
        {
            SceneManager.MoveGameObjectToScene(go, activeScene);
        }
    }

    IEnumerator LoadSceneAdditive(SceneFieldAsset scene)
    {
        if (!isprocessing)
        {
            isprocessing = true;
            //if (SphereFader == null)
            //{
            //    var temp = FindObjectOfType<FaderItem>();
            //    if(temp != null)
            //        SphereFader = FindObjectOfType<FaderItem>().gameObject;
            //}
            //if (SphereFader != null)
            //{
            //    FaderAnim = SphereFader.GetComponent<Animator>();
            //SphereFader.SetActive(true);
            //}
            // start ScreenFader + wait end if coroutine event
            //var _duration = 0.5f;
            //if (SphereFader != null)
            //{
            //    var TScale = SphereFader.transform.localScale;
            //SphereFader.transform.localScale = new Vector3(0, 0, 0);
            //    SphereFader.SetActive(true);
            //    FaderAnim.SetTrigger("Transparent");
            //    yield return new WaitForSeconds(_duration);
            //SphereFader.transform.localScale = TScale;
            //    FaderAnim.SetTrigger("Fade");
            //}
            //yield return new WaitForSeconds(_duration);
            var asyncOp = SceneManager.LoadSceneAsync(scene.m_sceneName, LoadSceneMode.Additive);
            while (!asyncOp.isDone)
            {
                //Debug.Log("Loading scene: " + (asyncOp.progress * 100).ToString() + " %");
                yield return null;
            }

            if (asyncOp.isDone)
            {
                countCurrentLoadedScenes++;
                tempLoadedScenes.Add(scene.SceneName);
            }
            if (countCurrentLoadedScenes >= countScenesToLoad)
            {
                if (SceneLoadCallback != null)
                    SceneLoadCallback();
                MoveObjectsToActiveScene();
                //forice set active scene
                SetLastLoadedSceneActiveScene();
                UnloadScenes();
                //SphereFader = FindObjectOfType<FaderItem>().gameObject;
                //FaderAnim = SphereFader.GetComponent<Animator>();
                //FaderAnim.SetTrigger("Transparent");
                //yield return new WaitForSeconds(_duration);
                //SphereFader.SetActive(false);
                isprocessing = false;
            }
        }
    }

    /*IEnumerator CoFader( bool isFadeIn)
    {
        var _duration = 0.5f;
        if (isFadeIn)
            FaderAnim.SetTrigger("Fade");
        else
            FaderAnim.SetTrigger("Transparent");
        yield return new WaitForSeconds(_duration);
        if(!isFadeIn)
            FaderAnim.gameObject.SetActive(false);
    }*/

    public void SetScenesToUnload(SceneFieldAsset[] scenesToUnload)
    {
        currentScenesToUnload = new List<SceneFieldAsset>();

        foreach (var scene in scenesToUnload)
        {
            currentScenesToUnload.Add(scene);
        }
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Scene loaded sucessufully: " + scene.name);

        if (lastLoadedSceneIsActive)
        {
            activeScene = scene;
            SetLastLoadedSceneActiveScene();
        }
    }

    void ActiveSceneChanged(Scene scene, Scene mode)
    {
        //Debug.Log("Active scene changed. Previour scene: " + scene.name);
    }

    void SceneUnloaded(Scene scene)
    {
        //Debug.Log("Scene " + scene.name + " unloaded");
    }
}

