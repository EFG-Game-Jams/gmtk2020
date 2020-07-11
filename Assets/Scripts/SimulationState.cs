using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationState : Singleton<SimulationState>
{
    public enum Mode
    {
        None,
        Edit,
        Simulate,
    }

    public Mode CurrentMode { get; private set; } = Mode.Edit;
    public LevelDescriptor CurrentLevel { get; private set; }

    private Dictionary<int, float> fuseSettings = new Dictionary<int, float>();

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadLevel(Mode mode, LevelDescriptor level = null)
    {
        level = level ?? CurrentLevel;

#if UNITY_EDITOR
        Resources.LoadAll("Levels");
        var levels = Resources.FindObjectsOfTypeAll<LevelDescriptor>();
        foreach (var levelAsset in levels)
        {
            if (levelAsset.sceneName == SceneManager.GetActiveScene().name)
                level = levelAsset;
            if (levelAsset.sceneIndex < 0)
                Debug.LogWarning("Level descriptor '" + levelAsset.name + "' references scene '" + levelAsset.sceneName + "' which is not included in build.");
        }
#endif

        if (CurrentMode == Mode.Edit)
            SaveFuseSettings();

        CurrentMode = mode;
        CurrentLevel = level;
        SceneManager.LoadScene(level.sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        switch (CurrentMode)
        {
            case Mode.None:
                ClearFuseSettings();
                break;
            case Mode.Edit:
                LoadFuseSettings();
                break;
            case Mode.Simulate:
                LoadFuseSettings();
                GetBombs().BroadcastMessage("StartSimulation");
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

    private void ClearFuseSettings()
    {
        fuseSettings.Clear();
    }
    private void SaveFuseSettings()
    {
        ClearFuseSettings();

        Transform root = GetBombs().transform;
        for (int i = 0; i < root.childCount; ++i)
        {
            Fuse fuse = root.GetChild(i).GetComponent<Fuse>();
            if (fuse == null || fuse.timeToDetonate <= 0)
                continue;
            fuseSettings.Add(i, fuse.timeToDetonate);
        }
    }
    private void LoadFuseSettings()
    {
        Transform root = GetBombs().transform;
        foreach (var pair in fuseSettings)
        {
            Fuse fuse = root.GetChild(pair.Key).GetComponent<Fuse>();
            fuse.SetTimeToDetonate(pair.Value);
        }
    }

    private GameObject GetBombs() => GameObject.Find("Bombs");
}
