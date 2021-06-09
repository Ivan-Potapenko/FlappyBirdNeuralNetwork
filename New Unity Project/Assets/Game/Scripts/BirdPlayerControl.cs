using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPlayerControl : MonoBehaviour
{
    [SerializeField]
    private BirdMove _birdMove;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _birdMove.Jump();
        }
    }
}
