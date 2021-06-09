using System.Collections.Generic;


public class NetworkData 
{

    public List<NeuronLayer> HidenLayers;
    public NeuronLayer InputLayer;
    public int Score;

    public NetworkData Copy()
    {
        var copy = new NetworkData();
        var hidenLayers = new List<NeuronLayer>();
        for(int i = 0; i<HidenLayers.Count;i++)
        {
            hidenLayers.Add(new NeuronLayer(HidenLayers[i].Neurons));
        }
        var inputLayers = new NeuronLayer(InputLayer.Neurons);
        copy.HidenLayers = hidenLayers;
        copy.InputLayer = inputLayers;
        return copy;
    }
}
