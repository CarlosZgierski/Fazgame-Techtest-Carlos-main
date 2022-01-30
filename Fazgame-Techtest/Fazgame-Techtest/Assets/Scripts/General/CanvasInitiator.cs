using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class CanvasInitiator : MonoBehaviour
{
	void Awake()
    {
		GetComponent<Canvas> ().enabled = true;
	}
}
