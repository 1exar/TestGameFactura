using System.Collections.Generic;
using TestGameFactura.Scripts.Configs.Levels;
using TestGameFactura.Scripts.Factories;
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

        private readonly List<GameObject> _stageObjects = new();

        [Inject]
        public LevelManager(
            LevelConfig levelConfig,
            EnemyFactory enemyFactory,
            [Inject(Id = "StagePrefab")] GameObject stagePrefab,
            [Inject(Id = "StageParent")] Transform stageParent,
            [Inject(Id = "StageLength")] float stageLength,
            [Inject(Id = "SpawnRange")] Vector2 spawnRange)
        {
            _levelConfig = levelConfig;
            _enemyFactory = enemyFactory;
            _stagePrefab = stagePrefab;
            _stageParent = stageParent;
            _stageLength = stageLength;
            _spawnRange = spawnRange;
        }

        public void Initialize()
        {
            LoadLevel(_levelConfig);
        }

        public void LoadLevel(LevelConfig config)
        {
            ClearLevel();
            GenerateLevel(config.Stages);
        }

        private void ClearLevel()
        {
            foreach (var obj in _stageObjects)
            {
                GameObject.Destroy(obj);
            }

            _stageObjects.Clear();
        }

        private void GenerateLevel(List<LevelStage> stages)
        {
            for (int stageIndex = 0; stageIndex < stages.Count; stageIndex++)
            {
                Vector3 stagePosition = Vector3.forward * (_stageLength * stageIndex);
                var stage = Object.Instantiate(_stagePrefab, stagePosition, _stagePrefab.transform.rotation, _stageParent);
                _stageObjects.Add(stage);

                SpawnEnemiesForStage(stagePosition.z, stages[stageIndex]);
            }
        }

        private void SpawnEnemiesForStage(float stageZCenter, LevelStage stage)
        {
            float spawnZMin = stageZCenter - (_stageLength / 2) + _spawnRange.y;
            float spawnZMax = stageZCenter + (_stageLength / 2) - _spawnRange.y;

            for (int i = 0; i < stage.enemiesCount; i++)
            {
                float x = Random.Range(-_spawnRange.x, _spawnRange.x);
                float z = Random.Range(spawnZMin, spawnZMax);
                Vector3 pos = new Vector3(x, 0f, z);

                _enemyFactory.Create(pos, stage.enemiesMaxHp);
            }
        }
    }

}
