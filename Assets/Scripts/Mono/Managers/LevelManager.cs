using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int _currentLevel;
    public int _lastLevel;
    public SubScene[] _subScenes;

    private SceneSystem _sceneSystem;
    private Entity _currentLvelEntity;

    private void Start()
    {
        _sceneSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SceneSystem>();
    }

    private void Reset()
    {
        Load(0);
    }

    public void Load(int level)
    {
        if (level > _lastLevel)
        {
            Reset();
            return;
        }
        Unload();
        _currentLevel = level;
        _currentLvelEntity = _sceneSystem.LoadSceneAsync(_subScenes[_currentLevel].SceneGUID);
    }

    public void Unload()
    {
        if (_sceneSystem != null)
        {
            _sceneSystem.UnloadScene(_currentLvelEntity);
        }
    }

}