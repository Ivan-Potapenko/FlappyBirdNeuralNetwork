using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newNetworkData", menuName = "NetworkData")]
public class DataScriptable :ScriptableObject   
{
    public List<NetworkData> NetwokDatas;
    public int generation;
}
