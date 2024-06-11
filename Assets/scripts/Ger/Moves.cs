using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class Moves : MonoBehaviour
{
    private Image _currentCheckerImage;
    [SerializeField] private UDictionary<CheckerType, Sprite> _checkersSpriteRef = new UDictionary<CheckerType, Sprite>();

    private void Awake() { _currentCheckerImage = GetComponent<Image>(); }
    public void ChangeSprite(CheckerType checkerType) { _currentCheckerImage.sprite = _checkersSpriteRef.FirstOrDefault(pair => pair.Key == checkerType).Value; }
}
