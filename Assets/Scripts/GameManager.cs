using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _creditsCanvasGroup;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SubtitlesManager _subtitles;

    private Vector3 _playerStartPosition;
    private CharacterController _cc;
    private bool _canPlayerStart;

    void Start()
    {
        _cc = FindFirstObjectByType<CharacterController>();
        _cc.enabled = false;
        _playerStartPosition = _cc.transform.position;
    }

    private void Update()
    {
        if (_canPlayerStart && Input.anyKey) StartGame();
    }

    public void StartGame()
    {
        _creditsCanvasGroup.SetActive(false);
        _audioSource.Play();
        _subtitles.InitializeSubtitles();
        _subtitles.StartSubtitles("first");
        _canPlayerStart = false;
        _cc.enabled = true;
    }

    public void EndGame()
    {
        _audioSource.Stop();
        _creditsCanvasGroup.SetActive(true);
        _cc.enabled = false;
        _cc.transform.position = _playerStartPosition;
    }

    public void UnlockStart()
    {
        _canPlayerStart = true;
    }
    
}
