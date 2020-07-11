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
        Completed,
    }

    public struct PlayState
    {
        public int fusedBombs;
        public int destroyedBombs;

        public Fuse[] allFuses;
        public Rigidbody[] allBombRigidBodies;
    }

    public Mode CurrentMode { get; private set; } = Mode.Edit;
    public LevelDescriptor CurrentLevel { get; private set; }
    public PlayState CurrentPlayState => playState;

    public bool CanFuseBomb => (CurrentPlayState.fusedBombs < CurrentLevel.bombsFuseable);
    public bool CompletedBronze => (CurrentPlayState.destroyedBombs >= CurrentLevel.bombsForBronze);
    public bool CompletedSilver => (CurrentPlayState.destroyedBombs >= CurrentLevel.bombsForSilver);
    public bool CompletedGold => (CurrentPlayState.destroyedBombs >= CurrentLevel.bombsForGold);

    private PlayState playState;
    private Dictionary<int, float> fuseSettings = new Dictionary<int, float>();

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        CurrentLevel = FindLevelDescriptorForCurrentScene();
    }

    public void ClearMode()
    {
        // this is called by level selection to ensure we're in the right state
        // by default we assume we're in edit mode to allow for testing levels directly in the editor
        CurrentMode = Mode.None;
        CurrentLevel = null;
        ClearPlayState();
    }
    
    private void ClearPlayState()
    {
        playState = new PlayState();
    }
    private void InitPlayStateForSimulation()
    {
        playState.allFuses = GetBombs().GetComponentsInChildren<Fuse>();
        playState.allBombRigidBodies = GetBombs().GetComponentsInChildren<Rigidbody>();        
    }
    private bool HasSimulationCompleted()
    {
        if (playState.allFuses == null)
            return false; // not done loading

        foreach (var fuse in playState.allFuses)
            if (fuse != null && fuse.IsLit)
                return false;
        foreach (var rb in playState.allBombRigidBodies)
            if (rb != null && !rb.IsSleeping())
                return false;
        return true;
    }

    public void OnBombFused() => ++playState.fusedBombs;
    public void OnBombDefused() => --playState.fusedBombs;
    public void OnBombDestroyed() => ++playState.destroyedBombs;

    private LevelDescriptor FindLevelDescriptorForCurrentScene()
    {
        LevelDescriptor level = null;

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

        return level;
    }

    private void Update()
    {
        if (CurrentMode != Mode.Simulate)
            return;

        if (HasSimulationCompleted())
        {
            CurrentMode = Mode.Completed;
            Debug.Log("Simulation completed");
            
            if (playState.destroyedBombs > CurrentLevel.highScore)
            {
                CurrentLevel.highScore = playState.destroyedBombs;
                CurrentLevel.Save();
            }
        }
    }

    public void LoadLevel(Mode mode, LevelDescriptor level = null)
    {
        level = level ?? CurrentLevel;

        if (CurrentMode == Mode.Edit)
            SaveFuseSettings();

        CurrentMode = mode;
        CurrentLevel = level;
        ClearPlayState();
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
                InitPlayStateForSimulation();
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

        playState.fusedBombs = fuseSettings.Count;
    }

    private GameObject GetBombs() => GameObject.Find("Bombs");
}
