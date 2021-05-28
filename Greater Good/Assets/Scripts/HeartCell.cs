using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartCell : MonoBehaviour
{
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    Image image;
	private void Start()
	{
		image = GetComponent<Image>();
	}
	public void TurnOn() { image.sprite = onSprite; }
	public void TurnOff() { image.sprite = offSprite; }
}
