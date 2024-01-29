using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [SerializeField, Header("ゲームオーバーUI")]
    private GameObject _gameOverUI;

    [SerializeField, Header("ゲームクリアUI")]
    private GameObject _gameClearUI;

    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        _ShowGameOberUI();
    }

    private void _ShowGameOberUI()
    {
        if (_player != null) return;

        _gameOverUI.SetActive(true);
    }

    public void ShowGameClearUI()
    {
        _gameClearUI.SetActive(true);
    }
}
