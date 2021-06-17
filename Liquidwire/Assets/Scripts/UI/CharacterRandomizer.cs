using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class CharacterRandomizer : MonoBehaviour
    {
        [SerializeField] private Sprite[] _background;
        [SerializeField] private Sprite[] _head;
        [SerializeField] private Sprite[] _clothes;
        [SerializeField] private Sprite[] _eyes;
        [SerializeField] private Sprite[] _mouth;
        [SerializeField] private Sprite[] _hair;
        [SerializeField] private Sprite[] _accessories;
        private Sprite[][] _assemblyPieces;
        [SerializeField] private Image[] _slots;

        private Color[] _dyedColours;
        private Color[] _natColours;

        private void Start()
        {
            _assemblyPieces = new [] { _background, _head, _clothes, _eyes, _mouth, _hair, _accessories } ;

            _natColours = new[]
            {
                // based on https://i.pinimg.com/originals/56/49/db/5649dbb8edb612843e68ddffabd19934.jpg
                new Color(9/255f, 8/255f, 6/255f),
                new Color(44/255f, 34/255f, 43/255f),
                new Color(59 /255f, 48 /255f, 36 /255f),
                new Color(78 /255f, 67 /255f, 63 /255f),
                new Color(80 /255f, 68 /255f, 68 /255f),
                new Color(109/255f, 78 /255f, 66 /255f),
                new Color(85 /255f, 72 /255f, 56 /255f),
                new Color(167/255f, 133/255f, 106/255f),
                new Color(184/255f, 151/255f, 120/255f),
                new Color(220/255f, 208/255f, 186/255f),
                new Color(222/255f, 188/255f, 153/255f),
                new Color(151/255f, 121/255f, 97 /255f),
                new Color(230/255f, 206/255f, 168/255f),
                new Color(229/255f, 200/255f, 168/255f),
                new Color(165/255f, 107/255f, 70 /255f),
                new Color(45 /255f, 85 /255f, 61 /255f),
                new Color(83 /255f, 61 /255f, 50 /255f),
                new Color(113/255f, 99 /255f, 90 /255f),
                new Color(183/255f, 166/255f, 158/255f),
                new Color(214/255f, 196/255f, 194/255f),
                new Color(255/255f, 24 /255f, 225/255f),
                new Color(202/255f, 191/255f, 177/255f),
                new Color(141/255f, 74 /255f, 67 /255f),
                new Color(181/255f, 82 /255f, 57 /255f),
            };
            _dyedColours = new[]
            { // based on https://www.color-hex.com/color-palette/14269
                new Color(158/255f, 193/255f, 228/255f),
                new Color(231/255f, 142/255f, 199/255f),
                new Color(161/255f, 242/255f, 99 /255f),
                new Color(244/255f, 102/255f, 102/255f),
                new Color(189/255f, 105/255f, 185/255f),
            };

            RandomizeFace();
        }

        public string RandomizeFace()
        {
            string outPutID = "";
            Color hairColour = GenerateHairColour();
            for (int i = 0; i < _assemblyPieces.Length; i++)
            {
                int randomDigit = Random.Range(0, _assemblyPieces[i].Length);
                _slots[i].sprite = _assemblyPieces[i][randomDigit];
                if (i == 5 || (i == 6 && randomDigit == 0))
                {
                    _slots[i].color = hairColour;
                }
                else if (i == 6)
                {
                    _slots[i].color = Color.white;
                }

                if (i == 6 && randomDigit == 1 && _slots[5].sprite == _assemblyPieces[4][3])
                {
                    _slots[5].sprite = _assemblyPieces[4][1]; // this is a specific exception for the spiky hair and the cap together
                }
                outPutID += randomDigit;
            }

            outPutID += " " + ColorUtility.ToHtmlStringRGB(hairColour);
            return outPutID;
        }

        public void SetFace(string ID)
        {
            ColorUtility.TryParseHtmlString(ID.Split(' ')[1], out Color hairColour);
            ID = ID.Split(' ')[0];
            for (int i = 0; i < _assemblyPieces.Length; i++)
            {
                _slots[i].sprite = _assemblyPieces[i][ID[i]];
                if (i == 5 || (i == 6 && ID[i] == 0))
                {
                    _slots[i].color = hairColour;
                }
                else if (i == 6)
                {
                    _slots[i].color = Color.white;
                }
                if (i == 6 && ID[i] == 1 && _slots[5].sprite == _assemblyPieces[4][3])
                {
                    _slots[5].sprite = _assemblyPieces[4][1]; // this is a specific exception for the spiky hair and the cap together
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.U))
            {
                RandomizeFace();
            }
        }

        private Color GenerateHairColour()
        {
            Color returnValue;
            if (Random.Range(0f, 1f) > .8f)
            {
                returnValue = _dyedColours[Random.Range(0, _dyedColours.Length)];
            }
            else
            {
                returnValue = _natColours[Random.Range(0, _natColours.Length)];
            }
            // these could then be further manipulated if the results feel too similar

            return returnValue;
        }
    }
}
