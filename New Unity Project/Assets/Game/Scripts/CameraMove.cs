using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private ScriptableFloat _speed;

    [SerializeField]
    private Rigidbody2D _rigidbody2d;


    void Move()
    {
        _rigidbody2d.velocity = new Vector2(_speed.value,0f);
    }

    void FixedUpdate()
    {
        Move();
    }
}
