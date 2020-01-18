using Assets.Scripts;
using Assets.Scripts.Components;
using Assets.Scripts.ECSHelpers;
using Assets.Scripts.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    #region SerializeFields

    [SerializeField]
    private Sprite targetSprite;

    [SerializeField]
    private Sprite playerSprite;

    [SerializeField]
    private float playerSpeed = 1f;

    [SerializeField]
    private Sprite bulletSprite;

    [SerializeField]
    private Sprite rocketSprite;

    [SerializeField]
    private float fireSpeed = 3.5f;

    [SerializeField]
    private GameObject scoreText;

    #endregion

    private float[] targetY = new float[] { -5.5f, 5.5f };

    private EntityManager entityManager;

    private float spawnTargetTimer;

    // Start is called before the first frame update
    private void Start()
    {
        entityManager = new EntityManager();
        
        SpawnPlayer();
        
        entityManager.RegisterSystem(new InputSystem());
        entityManager.RegisterSystem(new PlayerMovementSystem());
        entityManager.RegisterSystem(new PlayerFireSystem(bulletSprite, rocketSprite, fireSpeed));
        entityManager.RegisterSystem(new BulletMoveSystem());
        entityManager.RegisterSystem(new RocketMoveSystem());
        entityManager.RegisterSystem(new TargetMovementSystem(targetY));
        entityManager.RegisterSystem(new ScoreSystem());
        entityManager.RegisterSystem(new UISystem(scoreText.GetComponent<TextMeshProUGUI>()));
    }

    // Update is called once per frame
    private void Update()
    {
        entityManager.Update();

        spawnTargetTimer -= Time.deltaTime;
        if (spawnTargetTimer <= 0)
        {
            SpawnTarget();

            spawnTargetTimer = UnityEngine.Random.Range(0.5f, 3f);
        }
    }

    // Spawn player entity
    private void SpawnPlayer()
    {
        Type[] playerArchetype = new Type[] { typeof(SpriteRenderer),
            typeof(PlayerComponent) };

        GameObject playerEntity = entityManager.CreateEntity("playerEntity", playerArchetype);

        playerEntity.GetComponent<SpriteRenderer>().sprite = playerSprite;
        playerEntity.GetComponent<PlayerComponent>().PlayerSpeed = playerSpeed;
        playerEntity.GetComponent<PlayerComponent>().PlayerOffset = playerEntity.GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    // Spawn target entity
    private void SpawnTarget()
    {
        Type[] targetArchetype = new Type[] { typeof(SpriteRenderer),
            typeof(CircleCollider2D),
            typeof(TargetComponent) };

        GameObject targetEntity = entityManager.CreateEntity("targetEntity", targetArchetype);

        int i = UnityEngine.Random.Range(0, 2);
        targetEntity.GetComponent<Transform>().position = new Vector3(UnityEngine.Random.Range(5f, 8.5f), targetY[i]);
        targetEntity.GetComponent<SpriteRenderer>().sprite = targetSprite;
        targetEntity.GetComponent<TargetComponent>().TargetDirection = i == 0 ? 1 : -1;  // 1 -> Up, -1 -> Down
        targetEntity.GetComponent<TargetComponent>().TargetSpeed = UnityEngine.Random.Range(0.5f, 4f);
        targetEntity.GetComponent<CircleCollider2D>().radius = targetEntity.GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }
}
