using System;
using System.Collections.Generic;
using UnityEngine;

public class NeuronLayer 
{
   

    public List<List<double>> Neurons;

    public NeuronLayer(int thisLayerCount, int nextLayerCount, double maxWeightsValue, double minWeightsValue)
    {
        RandomFillingWeights(thisLayerCount, nextLayerCount, maxWeightsValue, minWeightsValue);
    }

    public NeuronLayer(List<List<double>> neurons)
    {
        Neurons = new List<List<double>>();
        for(int i = 0; i<neurons.Count;i++)
        {
            List<double> weight = new List<double>();
            for (int j = 0;j  < neurons[i].Count; j++)
            {
                weight.Add(neurons[i][j]);
            }
            Neurons.Add(weight);
        }
    }

    public void RandomChangeInWeights(double maxChangeValue, double minChangeValue, int probabilityOfWweightChange)
    {
        for (int i = 0; i < Neurons.Count; i++)
        {
            for (int j = 0; j < Neurons[i].Count; j++)
            {
                if(UnityEngine.Random.Range(0, probabilityOfWweightChange) == 0)
                {
                    Neurons[i][j] += UnityEngine.Random.Range((float)minChangeValue, (float)maxChangeValue);
                }
            }
        }
    }

    public void RandomFillingWeights(int thisLayerCount, int nextLayerCount, double maxWeightsValue, double minWeightsValue)
    {
        Neurons = new List<List<double>>();
        for (int i = 0; i < thisLayerCount; i++)
        {
            List<double> weights = new List<double>();
            for (int j = 0; j < nextLayerCount; j++)
            {
                weights.Add(UnityEngine.Random.Range((float)minWeightsValue, (float)maxWeightsValue));
            }
            Neurons.Add(weights);
        }
    }

    private double ActivationFunctionResult(double input)
    {
       // double output =  input/(1+Mathf.Abs((float)input));
        // double output = input / (1 + Mathf.Abs((float)input));
        double output = 1 / (1 + Mathf.Exp(-(float)input));


        return output;
    }

    public List<double> calculateOutputsOfNeurons(List<double> inputNeuronsData)
    {
        List<double> outputsNeuronsData = new List<double>();

        for (int j = 0; j < Neurons[0].Count; j++)
        {
            outputsNeuronsData.Add(0);
        }
        for (int i = 0; i < Neurons.Count; i++)
        {
            for (int j = 0; j < Neurons[i].Count; j++)
            {
                outputsNeuronsData[j] += Neurons[i][j] * ActivationFunctionResult(inputNeuronsData[i]);
            }
        }
        return outputsNeuronsData;
    }
}
