using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    private float _moveSpeed;
    [SerializeField, Header("ジャンプ速度")]
    private float _jumpSpeed;
    [SerializeField, Header("体力")]
    private int _hp;
    [SerializeField, Header("無敵時間")]
    private float _damageTime;
    [SerializeField, Header("点滅時間")]
    private float _flashTime;

    private Vector2 _InputDirection;
    private Rigidbody2D _rigid;
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    private bool _bJump;

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        _LookMoveDirec();
        Debug.Log(_hp);
        _HitFloor();
    }

    private void _Move()
    {
        //if (_bJump) return;
        _rigid.velocity = new Vector3(_InputDirection.x * _moveSpeed, _rigid.velocity.y);
        _anim.SetBool("Walk", _InputDirection.x != 0.0f);
    }

    private void _LookMoveDirec()
    {
        if (_InputDirection.x > 0.0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (_InputDirection.x < 0.0f)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Floor")
        //{
        //    _bJump = false;
        //    _anim.SetBool("Jump", _bJump);
        //}
        if (collision.gameObject.tag == "Enemy")
        {
            _HitEnemy(collision.gameObject);
        }
        else if(collision.gameObject.tag == "Goal")
        {
            FindObjectOfType<MainManager>().ShowGameClearUI();
            enabled = false;
            GetComponent<PlayerInput>().enabled = false;
        }
    }
    
    private void _HitFloor()
    {
        int layerMask = LayerMask.GetMask("Floor");
        Vector3 rayPos = transform.position - new Vector3(0.0f, transform.lossyScale.y / 2.0f);
        Vector3 raySIze = new Vector3(transform.lossyScale.x - 0.1f, 0.1f);
        RaycastHit2D rayHit = Physics2D.BoxCast(rayPos, raySIze, 0.0f, Vector2.zero, 0.0f, layerMask);
        if(rayHit.transform == null)
        {
            _bJump = true;
            _anim.SetBool("Jump", _bJump);
            return;
        }
        if(rayHit.transform.tag == "Floor" && _bJump)
        {
            _bJump = false;
            _anim.SetBool("Jump", _bJump);
        }
    }

    private void _HitEnemy(GameObject enemy)
    {
        float halfScaleY = transform.lossyScale.y / 2.0f;
        float enemyHalfScaleY = enemy.transform.lossyScale.y / 2.0f;
        if (transform.position.y - (halfScaleY - 0.1f) >= enemy.transform.position.y + (enemyHalfScaleY - 0.1f))
        {
            Destroy(enemy);
            _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        }
        else
        {
            enemy.GetComponent<BoundEnemy>().PlayerDamage(this);
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            StartCoroutine(_Damage());
        }
    }

    private void _Dead()
    {
        if(_hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator _Damage()
    {
        Color color = _spriteRenderer.color;
        for (int i = 0; i < _damageTime; i++)
        {
            yield return new WaitForSeconds(_flashTime);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 0.0f);

            yield return new WaitForSeconds(_flashTime);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f);
        }
        _spriteRenderer.color = color;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void _OnMove(InputAction.CallbackContext context)
    {
        _InputDirection = context.ReadValue<Vector2>();
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || _bJump) return;

        _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        //_bJump = true;
        //_anim.SetBool("Jump", _bJump);
    }

    public void Damage(int damege)
    {
        _hp = Mathf.Max(_hp - damege, 0);
        _Dead();
    }

    public int GetHP()
    {
        return _hp;
    }
}
