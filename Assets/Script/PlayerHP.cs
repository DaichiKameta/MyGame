using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField, Header("HPÉAÉCÉRÉì")]
    private GameObject _playerIcon;

    private Player _player;
    private int _beforeHP;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _beforeHP = _player.GetHP();
        _CreateHPIcon();
    }

    private void _CreateHPIcon()
    {
        for(int i = 0; i < _player.GetHP();i++)
        {
            GameObject _playerHPObj = Instantiate(_playerIcon);
            _playerHPObj.transform.SetParent(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _ShowHPIcon();
    }

    private void _ShowHPIcon()
    {
        if (_beforeHP == _player.GetHP()) return;
        UnityEngine.UI.Image[] icons = transform.GetComponentsInChildren<UnityEngine.UI.Image>();
        for(int i = 0; i < icons.Length;i++)
        {
            icons[i].gameObject.SetActive(i < _player.GetHP());
        }
        _beforeHP = _player.GetHP();
    }
}
