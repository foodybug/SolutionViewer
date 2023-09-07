using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour {

	[SerializeField] float timeOn = 1f;
	[SerializeField] float timeOff = 0.5f;

	IEnumerator Start()
	{
		Text txt = GetComponent<Text>();

		while(true)
		{
			txt.enabled = true;

			yield return new WaitForSeconds(timeOn);

			txt.enabled = false;

			yield return new WaitForSeconds(timeOff);
		}
	}
}
