using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class SubtitlesManager : MonoBehaviour
{
    [SerializeField] private GameObject _subtitlePanel;
    [SerializeField] private TextMeshProUGUI _subtitleText;
    [SerializeField] private TextAsset _inkAsset;
    [SerializeField] private float _textSpeed = 1f;
     
    private Story _story;
    private string _currentPathString;
    private bool _isDialoguePlaying;

    void Start()
    {
        _isDialoguePlaying = false;
        _subtitlePanel.SetActive(false);
        _story = new Story(_inkAsset.text);

        StartSubtitles("at_first");
    }

    public void StartSubtitles(string pathString)
    {
        _story.ChoosePathString(pathString);
        _isDialoguePlaying = true;
        _subtitlePanel.SetActive(true);
        TriggerNextLine();
    }

    private IEnumerator TriggerNextLine()
    {
        while (_story.canContinue)
        {
            float time = _story.currentText.Length * _textSpeed;
            _subtitleText.text = _story.Continue();

            yield return new WaitForSeconds(time);
        }
        _subtitlePanel.SetActive(false);
    }

}
