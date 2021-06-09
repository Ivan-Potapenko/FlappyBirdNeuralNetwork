using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkBirdGenerator : MonoBehaviour
{

    [SerializeField]
    private List<BirdNeuralNetwork> _birdNeuralNetworks;

    [SerializeField]
    private GameObject _birdNetworkPrefab;

    [SerializeField]
    private PipeGenerator _pipeGenerator;

    [SerializeField]
    private int _birdNetworksCount;

    [SerializeField]
    private bool _loadFromFile = true;

    [SerializeField]
    private bool reload = false;


    [SerializeField]
    private bool Stop = false;

    [SerializeField]
    private DataScriptable _networkDataScriptable;

    [SerializeField]
    private Text _text;

    [SerializeField]
    private int _numberOfNeuralNetworksForMixing;

    [SerializeField]
    private int _numberOfNeuralNetworksForRandomFilling;

    void Start()
    {
        _birdNeuralNetworks = new List<BirdNeuralNetwork>();
        LoadStart();
    }

    private void LoadStart()
    {
        _birdNeuralNetworks.Clear();


        if (_networkDataScriptable != null && _networkDataScriptable.NetwokDatas != null &&
            _networkDataScriptable.NetwokDatas[0].HidenLayers != null && _networkDataScriptable.NetwokDatas[0].Score > 100)
        {

            for (int i = 0; i < _numberOfNeuralNetworksForMixing; i++)
            {
                var _networkData_i = new NetworkData();
                _networkData_i.HidenLayers = _networkDataScriptable.NetwokDatas[i].HidenLayers;
                _networkData_i.InputLayer = _networkDataScriptable.NetwokDatas[i].InputLayer;
                _networkData_i.Score = _networkDataScriptable.NetwokDatas[i].Score;
                _birdNeuralNetworks.Add(Instantiate(_birdNetworkPrefab).GetComponent<BirdNeuralNetwork>());
                _birdNeuralNetworks[_birdNeuralNetworks.Count - 1].LoadStart(_networkData_i.Copy());
                _birdNeuralNetworks[_birdNeuralNetworks.Count - 1].PipeGenerator = _pipeGenerator;
            }
            MixNeuralNetworksAndRandom();
            RandomStart(_numberOfNeuralNetworksForRandomFilling);

            Debug.Log("загруженно");
            _networkDataScriptable.generation++;
            _text.text = "поколение: " + _networkDataScriptable.generation.ToString() + " счет: " +
                _networkDataScriptable.NetwokDatas[0].Score.ToString();
        }
        else
        {
            _networkDataScriptable.generation = 0;
            RandomStart(_birdNetworksCount);
        }
    }

    void MixNeuralNetworksAndRandom()
    {
        if (_birdNeuralNetworks == null)
        {
            _birdNeuralNetworks = new List<BirdNeuralNetwork>();
        }
        for (int i = 0; i < _numberOfNeuralNetworksForMixing - 1; i++)
        {
            for (int j = i + 1; j < _numberOfNeuralNetworksForMixing; j++)
            {
                MixNetwork(i, j);
            }

        }
        for (int i = 0; i < _numberOfNeuralNetworksForMixing - 1; i++)
        {
            for (int j = i + 1; j < _numberOfNeuralNetworksForMixing - 1; j++)
            {
                MixNetwork(i, j);
                _birdNeuralNetworks[_birdNeuralNetworks.Count - 1].RandomChangeInWeights();
            }

        }
    }


    void MixNetwork(int i, int j)
    {
        var _networkData_i = GetNetworkDataByIndex(i);
        var _networkData_j  = GetNetworkDataByIndex(j);
        _birdNeuralNetworks.Add(Instantiate(_birdNetworkPrefab).GetComponent<BirdNeuralNetwork>());
        _birdNeuralNetworks[_birdNeuralNetworks.Count - 1].MixLoad(_networkData_i.Copy(), _networkData_j.Copy());
        _birdNeuralNetworks[_birdNeuralNetworks.Count - 1].PipeGenerator = _pipeGenerator;
    }

    private NetworkData GetNetworkDataByIndex(int i)
    {
        var outputNetworkData = new NetworkData();
        outputNetworkData.HidenLayers = new List<NeuronLayer>(_networkDataScriptable.NetwokDatas[i].HidenLayers);
        outputNetworkData.InputLayer = _networkDataScriptable.NetwokDatas[i].InputLayer;
        outputNetworkData.Score = _networkDataScriptable.NetwokDatas[i].Score;
        return outputNetworkData;
    }

    private void RandomStart(int birdNetworksCount)
    {
        for (int i = 0; i < birdNetworksCount; i++)
        {
            _birdNeuralNetworks.Add(Instantiate(_birdNetworkPrefab).GetComponent<BirdNeuralNetwork>());
            _birdNeuralNetworks[_birdNeuralNetworks.Count - 1].RandomStart();
            _birdNeuralNetworks[_birdNeuralNetworks.Count - 1].PipeGenerator = _pipeGenerator;
        }
    }


    private void FixedUpdate()
    {
        if (!Stop)
        {
           Stop = IsAllBirdNetworksStop();

        }
        else
        {
            SaveBestBirdNetworks();
        }

    }

    private void SaveBestBirdNetworks()
    {
        if (_networkDataScriptable.NetwokDatas == null)
        {
            _networkDataScriptable.NetwokDatas = new List<NetworkData>();
        }

        SortNetworks();

        int k = 0;
        while (_numberOfNeuralNetworksForMixing > _networkDataScriptable.NetwokDatas.Count)
        {
            _networkDataScriptable.NetwokDatas.Add(_birdNeuralNetworks[k].GetNetworkData());
            k++;
        }

        for (int i = 0; i < _numberOfNeuralNetworksForMixing; i++)
        {
            if (_networkDataScriptable.NetwokDatas[i].Score < _birdNeuralNetworks[k].Score)
            {
                _networkDataScriptable.NetwokDatas[i] = _birdNeuralNetworks[k].GetNetworkData();
                k++;
            }
        }


        Stop = false;
        if (reload)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void SortNetworks()
    {
        for (int i = 0; i < _birdNeuralNetworks.Count - 1; i++)
        {
            for (int j = i + 1; j < _birdNeuralNetworks.Count; j++)
            {
                if (_birdNeuralNetworks[i].Score < _birdNeuralNetworks[j].Score)
                {
                    var neuralNetwork = _birdNeuralNetworks[i];
                    _birdNeuralNetworks[i] = _birdNeuralNetworks[j];
                    _birdNeuralNetworks[j] = neuralNetwork;
                }
            }
        }
    }

    private bool IsAllBirdNetworksStop()
    {
        for (int i = 0; i < _birdNeuralNetworks.Count; i++)
        {
            if (!_birdNeuralNetworks[i].Stop)
            {
                return false;
            }
        }
        return true;
    }
}
