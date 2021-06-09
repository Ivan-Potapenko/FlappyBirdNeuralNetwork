using System.Collections.Generic;
using UnityEngine;

public class BirdNeuralNetwork : MonoBehaviour
{
    [SerializeField]
    private BirdMove _bird;

    public PipeGenerator PipeGenerator;

    [SerializeField]
    private List<NeuronLayer> _hidenLayers;

    [SerializeField]
    private NeuronLayer _inputLayer;


    public double OutputData;

    [SerializeField]
    private List<int> _hidenNeuronsCount;

    [SerializeField]
    private double _minWeightStartValue;

    [SerializeField]
    private double _maxWeightStartValue;

    [SerializeField]
    private double _maxChangeValue;

    [SerializeField]
    private double _minChangeValue;

    [SerializeField]
    private int _probabilityOfWweightChange;

    [SerializeField]
    private PipeGenerator._doublePipes _pipe;

    public List<double> InputData;

    public bool Stop = false;
    public int Score;
    private bool StopUpdate = false;

    public NetworkData GetNetworkData()
    {
        NetworkData outputNetworkData = new NetworkData();
        outputNetworkData.HidenLayers = _hidenLayers;
        outputNetworkData.InputLayer = _inputLayer;
        outputNetworkData.Score = Score;
        return outputNetworkData;
    }

    private void Start()
    {
        Score = 0;
    }

    public void MixLoad(NetworkData networkData_i, NetworkData networkData_j)
    {
        if (Random.Range(0, 1) == 0)
        {
            for (int i = networkData_i.HidenLayers.Count / 2; i < networkData_i.HidenLayers.Count; i++)
            {
                networkData_i.HidenLayers[i] = networkData_j.HidenLayers[i];
            }
        }
        else
        {
            for (int i = networkData_i.HidenLayers.Count / 2; i < networkData_i.HidenLayers.Count; i++)
            {
                networkData_i.HidenLayers[i] = networkData_j.HidenLayers[i];
            }
        }
        if (Random.Range(0, 1) == 0)
        {
            networkData_i.InputLayer = networkData_j.InputLayer;
        }
        _inputLayer = networkData_i.InputLayer;
        _hidenLayers = networkData_i.HidenLayers;
        InputData = new List<double>();
    }
    public void LoadStart(NetworkData savedData)
    {
        _inputLayer = savedData.InputLayer;
        _hidenLayers = savedData.HidenLayers;
        InputData = new List<double>();
    }


    public void RandomStart()
    {
        _inputLayer = new NeuronLayer(2, _hidenNeuronsCount[0], _maxWeightStartValue, _minWeightStartValue);
        InputData = new List<double>();

        _hidenLayers = new List<NeuronLayer>();

        for (int i = 0; i < _hidenNeuronsCount.Count - 1; i++)
        {
            _hidenLayers.Add(new NeuronLayer(_hidenNeuronsCount[i], _hidenNeuronsCount[i + 1], _maxWeightStartValue, _minWeightStartValue));
        }

        _hidenLayers.Add(new NeuronLayer(_hidenNeuronsCount[_hidenNeuronsCount.Count - 1], 1, _maxWeightStartValue, _minWeightStartValue));

    }

    private void ReceiveInputData()
    {
        InputData.Clear();
        var pipes = new List<PipeGenerator._doublePipes>(PipeGenerator.Pipes);
        for (int i = pipes.Count - 1; i >= 0; i--)
        {
            if ((pipes[i].bottomPipe.transform.position.x - _bird.transform.localPosition.x) < -0.5f)
            {
                pipes.RemoveAt(i);
            }
        }

        for (int i = 0; i < pipes.Count - 1; i++)
        {
            for (int j = i + 1; j < pipes.Count; j++)
            {
                if (Mathf.Abs(pipes[i].bottomPipe.transform.position.x) > Mathf.Abs(pipes[j].bottomPipe.transform.position.x))
                {
                    var pipe = pipes[i];
                    pipes[i] = pipes[j];
                    pipes[j] = pipe;
                }
            }
        }
        if (pipes.Count == 3)
        {
            pipes.RemoveAt(2);
        }

        for (int i = 0; i < 1; i++)
        {
            if (pipes.Count > i)
            {
                InputData.Add(NormalizeFunctionResult((pipes[i].bottomPipe.transform.position.x - _bird.transform.localPosition.x)) * 2);
                InputData.Add(NormalizeFunctionResult(((pipes[i].bottomPipe.transform.position.y + pipes[i].upperPipe.transform.position.y) / 2 - _bird.transform.localPosition.y)) * 2);
            }
            else
            {
                InputData.Add(NormalizeFunctionResult(0));
                InputData.Add(NormalizeFunctionResult(0));
            }

        }
        if (pipes.Count <= 0)
        {
            InputData.Add(NormalizeFunctionResult(-20));
            InputData.Add(NormalizeFunctionResult(-20));
        }
    }

    private double NormalizeFunctionResult(double input)
    {
        double output = Mathf.Exp(-(float)(input * input));
        return output;
    }

    private double OutputFunctionResult(double input)
    {
        double output = 1 / (1 + Mathf.Exp(-(float)input));
        return output;
    }

    private void CalculateOutputData()
    {
        List<double> inputLayerOutput = _inputLayer.calculateOutputsOfNeurons(InputData);

        List<double> hidenLayerOutput = _hidenLayers[0].calculateOutputsOfNeurons(inputLayerOutput);
        for (int i = 1; i < _hidenLayers.Count; i++)
        {
            hidenLayerOutput = _hidenLayers[i].calculateOutputsOfNeurons(hidenLayerOutput);
        }
        OutputData = OutputFunctionResult(hidenLayerOutput[0]);
    }

    public void RandomChangeInWeights()
    {
        _inputLayer.RandomChangeInWeights(_maxChangeValue, _minChangeValue, _probabilityOfWweightChange);
        for (int i = 0; i < _hidenLayers.Count; i++)
        {
            _hidenLayers[i].RandomChangeInWeights(_maxChangeValue, _minChangeValue, _probabilityOfWweightChange);
        }
    }

    void FixedUpdate()
    {
        if (!StopUpdate)
        {
            if (_bird.Stop == false)
            {
                ReceiveInputData();
                CalculateOutputData();
                if (OutputData > 0.5f)
                {
                    _bird.Jump();
                }
            }
            else
            {

                StopUpdate = true;
                Stop = true;
                Score += _bird.Score;
                Destroy(_bird.gameObject);
                ReceiveInputData();
            }
        }
    }
}
