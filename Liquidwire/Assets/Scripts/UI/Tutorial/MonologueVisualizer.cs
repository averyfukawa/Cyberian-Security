using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace UI.Tutorial
{
    [RequireComponent(typeof(TextMeshProUGUI))] // functionally necessary
    [RequireComponent(typeof(Canvas))] // for performance reason - to minimize the amount of canvas elements being updated
    public class MonologueVisualizer : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private Queue<string> _outstandingBlocks = new Queue<string>();
        private Coroutine _currentRoutine;
        private Color revealColour = Color.black;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            if (_text.overflowMode != TextOverflowModes.Ellipsis)
            {
                Debug.LogWarning("please change overflow mode of " + _text.name + " to ellipsis");
            }

        }

        public float VisualizeText(string newText)
        {
            if (revealColour == Color.black)
            {
                revealColour = _text.color;
            }
            if (_currentRoutine != null)
            {
                StopCoroutine(_currentRoutine);
                _outstandingBlocks = new Queue<string>();
                _text.text = "";
                _text.color = revealColour;
            }
            _text.text = newText;
            // spit into substrings for each overflow, store those as queue, needs to wait for TMP execution first though
            StartCoroutine(WaitThenSplit());
            float estimatedTime = newText.Length*.05f + Mathf.Floor(newText.Length/25f);
            return estimatedTime;
        }

        public float VisualizeTextNonTutorial(string newText)
        {
            if (revealColour == Color.black)
            {
                revealColour = _text.color;
            }
            if (_currentRoutine != null && TutorialManager.Instance._doTutorial)
            {
                StopCoroutine(_currentRoutine);
                Debug.Log("text: "+ newText);
                _outstandingBlocks = new Queue<string>();
                _text.text = "";
                _text.color = revealColour;
            }
            else
            {
                Debug.Log("return.");
                return 0;
            }
            Debug.Log("text: "+ newText);
            _text.text = newText;
            // spit into substrings for each overflow, store those as queue, needs to wait for TMP execution first though
            StartCoroutine(WaitThenSplit());
            float estimatedTime = newText.Length*.05f + Mathf.Floor(newText.Length/25f);
            return estimatedTime;
        }
        private IEnumerator WaitThenSplit()
        {
            _text.color = new Color(0,0,0,0);
            yield return new WaitForEndOfFrame();
            while (_text.text.Length > 0 && _text.text.Length > _text.textInfo.lineInfo[_text.textInfo.lineCount - 1].lastCharacterIndex)
            {
                string block = _text.text.Substring(0, _text.textInfo.lineCount > 2 ? _text.textInfo.lineInfo[_text.textInfo.lineCount - 2].lastCharacterIndex : _text.text.Length);
                _outstandingBlocks.Enqueue(block);
                _text.text = _text.text.Substring(block.Length).TrimStart(new char[]{' ', '\n'});
                _text.ForceMeshUpdate();
            }

            _text.color = revealColour;
            // start coroutine that while loops the length of the queue and displays those letter by letter by changing individual mesh alpha
            _currentRoutine = StartCoroutine(TypewriterStyleReveal());
        }

        private IEnumerator TypewriterStyleReveal()
        {
        
            while (_outstandingBlocks.Count > 0)
            {
                _text.text = _outstandingBlocks.Dequeue();
                _text.color = new Color(0,0,0,0);
                _text.ForceMeshUpdate();
                int currentlyRevealed = 0;
                while (currentlyRevealed < _text.text.Length)
                {
                    int meshIndex = _text.textInfo.characterInfo[currentlyRevealed].materialReferenceIndex;
                    int vertexIndex = _text.textInfo.characterInfo[currentlyRevealed].vertexIndex;
                    Color32[] vertexColors = _text.textInfo.meshInfo[meshIndex].colors32;
                    vertexColors[vertexIndex + 0] = revealColour;
                    vertexColors[vertexIndex + 1] = revealColour;
                    vertexColors[vertexIndex + 2] = revealColour;
                    vertexColors[vertexIndex + 3] = revealColour;
                    currentlyRevealed++;
                    _text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                    
                    yield return new WaitForSeconds(.05f);
                }
                yield return new WaitForSeconds(1f);
            }

            _text.text = "";
            _text.color = revealColour;
            Debug.Log("finished monologue");
        }
    }
}
