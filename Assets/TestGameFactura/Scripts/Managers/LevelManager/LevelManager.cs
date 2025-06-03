using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TestGameFactura.Scripts.Configs.Levels;
using TestGameFactura.Scripts.Factories;
using TestGameFactura.Scripts.Managers.UIManager;
using Unity.AI.Navigation;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace TestGameFactura.Scripts.Managers.LevelManager
{
    public class LevelManager : IInitializable
    {
        private readonly LevelConfig _levelConfig;
        private readonly EnemyFactory _enemyFactory;
        private readonly GameObject _stagePrefab;
        private readonly Transform _stageParent;
        private readonly float _stageLength;
        private readonly Vector2 _spawnRange;

        private readonly NavMeshSurface _navMeshSurface;
        
        private readonly List<GameObject> _stageObjects = new();
        
        private readonly IUIManager _uiManager;

        [Inject]
        public LevelManager(
            LevelConfig levelConfig,
            EnemyFactory enemyFactory,
            [Inject(Id = "StagePrefab")] GameObject stagePrefab,
            [Inject(Id = "StageParent")] Transform stageParent,
            [Inject(Id = "StageLength")] float stageLength,
            [Inject(Id = "SpawnRange")] Vector2 spawnRange,
            NavMeshSurface navMeshSurface,
            IUIManager uiManager)
            
        {
            _levelConfig = levelConfig;
            _enemyFactory = enemyFactory;
            _stagePrefab = stagePrefab;
            _stageParent = stageParent;
            _stageLength = stageLength;
            _spawnRange = spawnRange;
            _navMeshSurface = navMeshSurface;
            _uiManager = uiManager;
        }

        public void Initialize()
        {
            InitializeAsync().Forget();
        }
        
        public async UniTaskVoid InitializeAsync()
        {
            await LoadLevelAsync(_levelConfig);
        }

        private async UniTask LoadLevelAsync(LevelConfig config)
        {
            ClearLevel();
            var stageData = await GenerateLevelAsync(config.Stages);
            _navMeshSurface.BuildNavMesh();
            await SpawnAllEnemiesAsync(stageData);
            _uiManager.ShowTransition(true);
        }

        private void ClearLevel()
        {
            foreach (var obj in _stageObjects)
            {
                Object.Destroy(obj);
            }

            _stageObjects.Clear();
            _navMeshSurface.RemoveData();
        }

        private async UniTask<List<(float zCenter, LevelStage stage)>> GenerateLevelAsync(List<LevelStage> stages)
        {
            var stageData = new List<(float zCenter, LevelStage stage)>();
            for (int stageIndex = 0; stageIndex < stages.Count; stageIndex++)
            {
                Vector3 stagePosition = _stageParent.transform.position + Vector3.forward * (_stageLength * stageIndex);
                var stageObj = Object.Instantiate(_stagePrefab, stagePosition, _stagePrefab.transform.rotation, _stageParent);
                _stageObjects.Add(stageObj);
                stageData.Add((stagePosition.z, stages[stageIndex]));
                await UniTask.Yield();
            }
            return stageData;
        }

        private async UniTask SpawnAllEnemiesAsync(List<(float zCenter, LevelStage stage)> stageData)
        {
            foreach (var (zCenter, stage) in stageData)
            {
                await SpawnEnemiesForStageAsync(zCenter, stage);
            }
        }

        private async UniTask SpawnEnemiesForStageAsync(float stageZCenter, LevelStage stage)
        {
            float spawnZMin = stageZCenter - (_stageLength / 2) + _spawnRange.y;
            float spawnZMax = stageZCenter + (_stageLength / 2) - _spawnRange.y;

            for (int i = 0; i < stage.enemiesCount; i++)
            {
                float x = Random.Range(-_spawnRange.x, _spawnRange.x);
                float z = Random.Range(spawnZMin, spawnZMax);
                Vector3 pos = new Vector3(x, 0f, z);

                _enemyFactory.Create(pos);
                await UniTask.Yield();
            }
        }
    }

}
