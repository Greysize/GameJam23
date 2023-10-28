using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class SceneFieldAsset
{
    public Scene m_sceneAsset2;
    public Object m_sceneAsset;
    public string m_sceneName = "";

    public string SceneName
    {
        get { return m_sceneName; }
    }

    public static implicit operator string(SceneFieldAsset sceneField)
    {
        return sceneField.SceneName;
    }
}
