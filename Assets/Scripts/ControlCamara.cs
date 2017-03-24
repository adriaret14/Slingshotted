using UnityEngine;
using System.Collections;

public class ControlCamara : MonoBehaviour {

    public GameObject objetivo;
    private Vector3 posicion;
    public float velocidad;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        posicion = new Vector3(objetivo.transform.position.x, objetivo.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp (transform.position, posicion, velocidad*Time.deltaTime);
	}
}
