﻿using System;
using TestGameFactura.Scripts.Configs.Enemy;
using TestGameFactura.Scripts.Configs.Levels;
using TestGameFactura.Scripts.Configs.Player;
using TestGameFactura.Scripts.Configs.Sound;
using TestGameFactura.Scripts.Pools;
using TestGameFactura.Scripts.Pools.Bullet;
using TestGameFactura.Scripts.Pools.Enemy;
using TestGameFactura.Scripts.Pools.Sound;
using UnityEngine;
using UnityEngine.Serialization;

namespace TestGameFactura.Scripts.Configs.Game
{
    [Serializable]
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
    public class GameConfig : ScriptableObject
    {
        
        [Header("Level Settings")]
        [SerializeField] private LevelConfig levelConfig;

        [Header("Player Settings")]
        [SerializeField] private PlayerTurretConfig playerTurretConfig;
        [SerializeField] private BulletPool bulletPool;
        [SerializeField] private int playerMaxHealth;
        [SerializeField] private float playerSpeed;

        [Header("Enemy Settings")]
        [SerializeField] private EnemyConfig enemyConfig;
        [FormerlySerializedAs("enemiesPoolDi")] [SerializeField] private EnemiesPool enemiesPool;

        [Header("Sound Config")]
        [SerializeField] private SoundConfig soundConfig;
        [SerializeField] private SoundPool soundPool;
        
        public LevelConfig LevelConfig => levelConfig;

        public PlayerTurretConfig PlayerTurretConfig => playerTurretConfig;
        public BulletPool BulletPool => bulletPool;
        public float PlayerSpeed => playerSpeed;
        public int PlayerMaxHealth => playerMaxHealth;

        public EnemyConfig EnemyConfig => enemyConfig;
        public EnemiesPool EnemiesPool => enemiesPool;
        
        public SoundConfig SoundConfig => soundConfig;
        public SoundPool SoundPool => soundPool;

    }
    
}