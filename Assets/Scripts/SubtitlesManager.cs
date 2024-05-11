using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using System.Linq;

public class SubtitlesManager : MonoBehaviour
{
    [SerializeField] private GameObject _subtitlePanel;
    [SerializeField] private TextMeshProUGUI _subtitleText;
    [SerializeField] private TextAsset _inkAsset;
    [SerializeField] private float _textSpeed = 1f;

    [SerializeField] private List<string> _basePathStrings;
    private List<string> _shuffledPathStrings;
     
    private Story _story;
    private string _currentPathString;
    private bool _isDialoguePlaying;

    void Start()
    {
        // shuffle a copy of the list
        _shuffledPathStrings = _basePathStrings.ToList();
        Debug.Log(_shuffledPathStrings);

        Shuffle(_shuffledPathStrings);


        _isDialoguePlaying = false;
        _subtitlePanel.SetActive(false);
        _story = new Story(_inkAsset.text);

        StartSubtitles("first");
        _shuffledPathStrings.Remove("first");
    }

    public void StartSubtitles(string pathString)
    {
        _story.ChoosePathString(pathString);
        _isDialoguePlaying = true;
        StartCoroutine(TriggerNextLine());
    }

    private IEnumerator TriggerNextLine()
    {
        Debug.Log(_shuffledPathStrings.Count);
        if (_shuffledPathStrings.Count == 0) yield break;

        // display new text and waits between lines
        while (_story.canContinue)
        {
            _subtitlePanel.SetActive(true);
            float time = _story.currentText.Length * _textSpeed + .2f;
            _subtitleText.text = _story.Continue();

            yield return new WaitForSeconds(time);
            _subtitlePanel.SetActive(false);

            yield return new WaitForSeconds(0.5f);
        }
        
        // waits longer between blocks
        _subtitlePanel.SetActive(false);
        yield return new WaitForSeconds(2f);

        // goes recursively through the whole list
        _currentPathString = Pluck(_shuffledPathStrings);
        _story.ChoosePathString(_currentPathString);
        StartCoroutine(TriggerNextLine());
    }

    private void Shuffle(List<string> list)
    {
        Debug.Log("Shuffling");
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            string value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private string Pluck(List<string> list)
    {
        Debug.Log("plucking");
        // pick a random path string
        int currentIndex = Random.Range(0, list.Count);
        string pluckedString = list[currentIndex];
        list.RemoveAt(currentIndex);

        return pluckedString;
    }
}
