using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
	public enum LightColor
	{
		Red,
		Yellow,
		Green
	}

	public GameObject[] lights;
	public LightColor lightColor;

	public int lightDirection { private get; set; } = 1;

	private void Awake()
	{
		foreach(GameObject light in lights)
		{
			light.SetActive(false);
		}
	}

	public void NextLight()
	{
		if ((int)lightColor >= lights.Length - 1 || (int)lightColor <= 0)
		{
			lightDirection *= -1;
		}
		
		ChangeLight(lightColor + lightDirection);
	}

	public void ChangeLight(LightColor color)
	{
		lights[(int)lightColor].SetActive(false);
		lights[(int)color].SetActive(true);

		lightColor = color;
	}
}
