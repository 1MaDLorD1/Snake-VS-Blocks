using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform _container;
    [SerializeField] private int _repeatCount;
    [SerializeField] private float _screenLength;
    [Header("Distance")]
    [SerializeField] private int _distanceBetweenFullLine;
    [SerializeField] private int _distanceBetweenRandomLine;
    [SerializeField] private int _distanceBetweenBonusLine;
    [Header("Block")]
    [SerializeField] private Block _blockTemplate;
    [SerializeField] private int _blockSpawnChance;
    [Header("Bonus")]
    [SerializeField] private Bonus _bonusTemplate;
    [SerializeField] private int _bonusSpawnChance;
    [Header("Wall")]
    [SerializeField] private Wall _wallTemplate;
    [SerializeField] private int _wallSpawnChance;

    private BlockSpawnPoint[] _blockSpawnPoints;
    private BonusSpawnPoint[] _bonusSpawnPoints;
    private WallSpawnPoint[] _wallSpawnPoints;
    private SideWallSpawnPoint[] _sideWallSpawnPoints;

    private void Start()
    {
        _blockSpawnPoints = GetComponentsInChildren<BlockSpawnPoint>();
        _bonusSpawnPoints = GetComponentsInChildren<BonusSpawnPoint>();
        _wallSpawnPoints = GetComponentsInChildren<WallSpawnPoint>();
        _sideWallSpawnPoints = GetComponentsInChildren<SideWallSpawnPoint>();

        GenerateSideWalls(_sideWallSpawnPoints, _wallTemplate, _screenLength);

        for (int i = 0; i < _repeatCount; i++)
        {
            MoveSpawner(_distanceBetweenBonusLine);
            GenerateRandomElements(_bonusSpawnPoints, _bonusTemplate.gameObject, _bonusSpawnChance, _bonusTemplate.transform.localScale.y);
            GenerateSideWalls(_sideWallSpawnPoints, _wallTemplate, _screenLength);
            MoveSpawner(_distanceBetweenFullLine);
            GenerateRandomElements(_wallSpawnPoints, _wallTemplate.gameObject, _wallSpawnChance, _distanceBetweenFullLine, _distanceBetweenFullLine / 2f);
            GenerateFullLine(_blockSpawnPoints, _blockTemplate.gameObject);
            GenerateSideWalls(_sideWallSpawnPoints, _wallTemplate, _screenLength);
            MoveSpawner(_distanceBetweenRandomLine);
            GenerateRandomElements(_wallSpawnPoints, _wallTemplate.gameObject, _wallSpawnChance, _distanceBetweenRandomLine, _distanceBetweenRandomLine / 2f);
            GenerateRandomElements(_blockSpawnPoints, _blockTemplate.gameObject, _blockSpawnChance, _blockTemplate.transform.localScale.y);
            GenerateSideWalls(_sideWallSpawnPoints, _wallTemplate, _screenLength);
        }
    }

    private void GenerateFullLine(SpawnPoint[] spawnPoints, GameObject generatedElement)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GenerateElement(spawnPoints[i].transform.position, generatedElement);
        }
    }

    private void GenerateRandomElements(SpawnPoint[] spawnPoints, GameObject generatedElement, int spawnChance, float scaleY = 1, float offsetY = 0)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if(Random.Range(0, 100) < spawnChance)
            {
                GameObject element = GenerateElement(spawnPoints[i].transform.position, generatedElement, offsetY);
                element.transform.localScale = new Vector3(element.transform.localScale.x, scaleY, element.transform.localScale.z);
            }
        }
    }

    private GameObject GenerateElement(Vector3 spawnPoint, GameObject generatedElement, float offsetY = 0)
    {
        spawnPoint.y -= offsetY;
        return Instantiate(generatedElement, spawnPoint, Quaternion.identity, _container);
    }

    private void MoveSpawner(int distanceY)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + distanceY, transform.position.z);
    }

    private void GenerateSideWalls(SpawnPoint[] spawnPoints, Wall wall, float screenLength)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject element = Instantiate(wall.gameObject, spawnPoints[i].transform.position, Quaternion.identity, _container);
            element.transform.localScale = new Vector3(element.transform.localScale.x, screenLength, element.transform.localScale.z);
        }
    }
}
