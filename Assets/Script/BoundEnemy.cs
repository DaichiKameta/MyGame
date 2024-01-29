using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundEnemy : MonoBehaviour
{
    [SerializeField, Header("à⁄ìÆë¨ìx")]
    private float _moveSpeed;
    [SerializeField, Header("çUåÇóÕ")]
    private int _attackPower;

    private Rigidbody2D _rigid;
    private Vector2 _moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _moveDirection = Vector2.left;
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        _ChangeMoveDirection();
        _LookMoveDirection();
    }

    private void _Move()
    {
        _rigid.velocity = new Vector2(_moveDirection.x * _moveSpeed, _rigid.velocity.y);
    }

    private void _ChangeMoveDirection()
    {
        Vector2 halfSize = transform.lossyScale / 2.0f;
        int layerMask = LayerMask.GetMask("Floor");
        RaycastHit2D ray = Physics2D.Raycast(transform.position, -transform.right, halfSize.x + 0.1f, layerMask);
        if(ray.transform == null) return;
        if(ray.transform.tag == "Floor")
        {
            _moveDirection = -_moveDirection;
        }
    }

    private void _LookMoveDirection()
    {
        if( _moveDirection.x < 0.0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(_moveDirection.x > 0.0f)
        {
            transform.eulerAngles = new Vector3(0.0f,180.0f,0.0f);
        }
    }

    public void PlayerDamage(Player player)
    {
        player.Damage(_attackPower);
    }
}
