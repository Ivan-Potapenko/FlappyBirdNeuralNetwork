using UnityEngine;

public class BirdMove : MonoBehaviour
{
    [SerializeField]
    private float _jumpForce;

    [SerializeField]
    private ScriptableFloat _speed;

    [SerializeField]
    private ScriptableFloat _score;


    public int Score = 0;

    [SerializeField]
    private Rigidbody2D _rigidbody2D;

    [SerializeField]
    private bool _stop = false;

    [SerializeField]
    private float startX = 0;

    public bool Stop { get => _stop; }


    public void Jump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pipe")
        {
            _stop = true;
            _rigidbody2D.simulated = false;
            Score = (int)(_score.value + gameObject.transform.position.x * 10);
        }
    }

    void FixedUpdate()
    {
        if (!Stop)
        {
            _rigidbody2D.velocity = new Vector2(_speed.value, _rigidbody2D.velocity.y);
        }

    }
}
