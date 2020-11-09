using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightManager : MonoBehaviour
{
	public TrafficLight[] trafficLights;
	public TrafficLight.LightColor[] trafficLinesColor = { TrafficLight.LightColor.Red, TrafficLight.LightColor.Green };
	public float[] lightColorStateDelay = { 5, 2, 5 };

	void Start()
	{
		InitializeTrafficLigts(trafficLights, 0, trafficLights.Length / 2, trafficLinesColor[0]);
		InitializeTrafficLigts(trafficLights, trafficLights.Length / 2, trafficLights.Length, trafficLinesColor[1]);

		StartCoroutine(LightChanger());
	}

	void InitializeTrafficLigts(TrafficLight[] trafficLights, int firstIndex, int lastIndex, TrafficLight.LightColor lightColor)
	{
		for (int i = firstIndex; i < lastIndex; i++)
		{
			trafficLights[i].ChangeLight(lightColor);
			trafficLights[i].lightDirection = lightColor == TrafficLight.LightColor.Red ? -1 : 1;
		}
	}

	IEnumerator LightChanger()
	{
		while (true)
		{
			yield return new WaitForSeconds(lightColorStateDelay[(int)trafficLights[0].lightColor]);

			foreach (TrafficLight trafficLight in trafficLights)
			{
				trafficLight.NextLight();
			}
		}
	}
}
