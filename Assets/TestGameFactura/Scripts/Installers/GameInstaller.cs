using TestGameFactura.Scripts.Configs.Game;
using TestGameFactura.Scripts.Configs.Levels;
using TestGameFactura.Scripts.Configs.Player;
using TestGameFactura.Scripts.Entities.Interfaces.Health;
using TestGameFactura.Scripts.Entities.Player;
using TestGameFactura.Scripts.Factories;
using TestGameFactura.Scripts.Managers.GameManagers;
using TestGameFactura.Scripts.Managers.LevelManager;
using TestGameFactura.Scripts.Pools;
using Unity.AI.Navigation;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private Transform levelParent;
        [SerializeField] private NavMeshSurface navMeshSurface;
        
        public override void InstallBindings()
        {
            // GameConfig
            Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle();

            // LevelConfig 
            Container.Bind<LevelConfig>().FromInstance(gameConfig.LevelConfig).AsSingle();

            // Factories
            Container.Bind<EnemyFactory>().AsSingle()
                .WithArguments(gameConfig.EnemyConfig);
            Container.Bind<BulletFactory>().AsSingle()
                .WithArguments(gameConfig.PlayerTurretConfig.BulletPrefab);

            // LevelManager
            Container.Bind<LevelManager>().AsSingle().NonLazy();
            Container.Bind<IInitializable>().To<LevelManager>().FromResolve();

            // Prefabs
            Container.Bind<GameObject>().WithId("StagePrefab").FromInstance(gameConfig.LevelStagePrefab);
            Container.Bind<float>().WithId("StageLength").FromInstance(gameConfig.StageLength);
            Container.Bind<Vector2>().WithId("SpawnRange").FromInstance(gameConfig.EnemySpawnRange);
            Container.Bind<Transform>().WithId("StageParent").FromInstance(levelParent);
            
            // Player
            Container.Bind<PlayerController>().FromInstance(player).AsSingle().NonLazy();
            Container.Bind<IHealth>().FromInstance(player).AsSingle();
            Container.Bind<PlayerTurretConfig>().FromInstance(gameConfig.PlayerTurretConfig).AsSingle();
            
            //Game Manager
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
            
            //NavMesh
            Container.Bind<NavMeshSurface>().FromInstance(navMeshSurface).AsSingle();
            
            //Pools 
            Container.Bind<EnemiesPool>().FromInstance(gameConfig.EnemiesPool).AsSingle();
            gameConfig.EnemiesPool.Init(Container);
        }
    } 
}
