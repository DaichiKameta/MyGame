using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Header("à⁄ìÆë¨ìx")]
    private float _moveSpeed;
    [SerializeField, Header("çUåÇóÕ")]
    private int _attackPower;

    private Rigidbody2D _rigid;
    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
    }

    private void _Move()
    {
        _rigid.velocity = new Vector2(Vector2.left.x * _moveSpeed, _rigid.velocity.y);
    }

    public void PlayerDamage(Player player)
    {
        player.Damage(_attackPower);
    }
}

