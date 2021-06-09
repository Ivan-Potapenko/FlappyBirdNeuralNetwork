using System.Collections.Generic;
using UnityEngine;

public class PipeGenerator : MonoBehaviour
{

    public struct _doublePipes
    {
        public GameObject upperPipe;
        public GameObject bottomPipe;
    }

    [SerializeField]
    private GameObject _mainCamera;

    [SerializeField]
    private GameObject _pipePrefab;

    [SerializeField]
    private List<_doublePipes> pipes;

    [SerializeField]
    private float _spawnDistanceOfPipesFromCameraX;

    [SerializeField]
    private float _maxUpperPipeDisplacementY;

    [SerializeField]
    private float _maxDisplacementBetweenPipesY;

    [SerializeField]
    private float _starUpperPipePositionY;

    [SerializeField]
    private float _maxUpperPipePositionY;

    [SerializeField]
    private float _minUpperPipePositionY;

    [SerializeField]
    private float _startDistanceBetweenPipesY;

    [SerializeField]
    private float _distanceFromCameraToSpawnPipesX;

    [SerializeField]
    private float _maxPipes;

    [SerializeField]
    private float _maxDistanceBetweenPipesY;

    [SerializeField]
    private ScriptableFloat _score;

    public List<_doublePipes> Pipes { get => pipes; }

    void Start()
    {
        _score.value = 0;       
        AddNewPipes();
    }

    private void AddNewPipes()
    {
        
        if (pipes == null)
        {
            pipes = new List<_doublePipes>();
        }

        float upperPipePositionY = _starUpperPipePositionY;
        float distanceBetweenPipesY = _startDistanceBetweenPipesY;

        if (pipes.Count != 0)
        {

            upperPipePositionY = pipes[pipes.Count - 1].upperPipe.transform.position.y + Random.Range(-_maxUpperPipeDisplacementY, _maxUpperPipeDisplacementY);
            if (upperPipePositionY > _maxUpperPipePositionY)
            {
                upperPipePositionY = _maxUpperPipePositionY;
            }
            else
            if (upperPipePositionY < _minUpperPipePositionY)
            {
                upperPipePositionY = _minUpperPipePositionY;

            }
            distanceBetweenPipesY = _startDistanceBetweenPipesY + Random.Range(0, _maxDisplacementBetweenPipesY);

            if (distanceBetweenPipesY > _maxDistanceBetweenPipesY)
            {
                distanceBetweenPipesY = _maxDistanceBetweenPipesY;
            }
        }

        _doublePipes doublePipes;
        Vector2 upperPipePosition = new Vector2(_mainCamera.transform.position.x + _spawnDistanceOfPipesFromCameraX, upperPipePositionY);
        Vector2 bottomPipePosition = new Vector2(_mainCamera.transform.position.x + _spawnDistanceOfPipesFromCameraX, upperPipePositionY - distanceBetweenPipesY);

        doublePipes.upperPipe = Instantiate(_pipePrefab, upperPipePosition, Quaternion.identity);
        doublePipes.bottomPipe = Instantiate(_pipePrefab, bottomPipePosition, Quaternion.identity);

        pipes.Add(doublePipes);
        if (pipes.Count > _maxPipes)
        {
            RemoveFirstPipes();
        }
       
        _score.value++;

    }

    void RemoveFirstPipes()
    {
        Destroy(pipes[0].upperPipe);
        Destroy(pipes[0].bottomPipe);
        pipes.RemoveAt(0);
    }

    void Update()
    {
        if (pipes[pipes.Count - 1].bottomPipe.transform.position.x - _mainCamera.transform.position.x <= _distanceFromCameraToSpawnPipesX)
        {
            AddNewPipes();
        }
    }
}
