using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour {


    public Image barra;

	public void useStamina()
    {
        barra.rectTransform.localScale += new Vector3(-1, 0, 0);
    }
    public void regenStamina()
    {
        barra.rectTransform.localScale = new Vector3(0.2F, 0, 0);
    }
}
