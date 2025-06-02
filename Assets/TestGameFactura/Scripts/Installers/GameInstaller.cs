using TestGameFactura.Scripts.Configs.Game;
using TestGameFactura.Scripts.Configs.Levels;
using TestGameFactura.Scripts.Configs.Player;
using TestGameFactura.Scripts.Factories;
using TestGameFactura.Scripts.Managers.LevelManager;
using TestGameFactura.Scripts.Player;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private Transform levelParent;

        public override void InstallBindings()
        {
            // GameConfig
            Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle();

            // LevelConfig 
            Container.Bind<LevelConfig>().FromInstance(gameConfig.LevelConfig).AsSingle();

            // EnemyFactory
            Container.Bind<EnemyFactory>().AsSingle()
                .WithArguments(gameConfig.EnemyPrefab);

            // LevelManager
            Container.Bind<LevelManager>().AsSingle().NonLazy();
            Container.Bind<IInitializable>().To<LevelManager>().FromResolve();

            // Prefabs
            Container.Bind<GameObject>().WithId("StagePrefab").FromInstance(gameConfig.LevelStagePrefab);
            Container.Bind<float>().WithId("StageLength").FromInstance(gameConfig.StageLength);
            Container.Bind<Vector2>().WithId("SpawnRange").FromInstance(gameConfig.EnemySpawnRange);
            Container.Bind<Transform>().WithId("StageParent").FromInstance(levelParent);
            
            // Player
            Container.Bind<PlayerController>().FromInstance(player).AsSingle();
            Container.Bind<PlayerTurretConfig>().FromInstance(gameConfig.PlayerTurretConfig).AsSingle();
        }
    } 
}
