using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PosicionMinimapa : MonoBehaviour {
    [SerializeField]
    private float fillAmount;

    [SerializeField]
    private Image content1, content2, content3, content4, content5, content6;

    // Use this for initialization
    void Start()
    {
        content1.fillAmount = 0;
        content2.fillAmount = 0;
        content3.fillAmount = 0;
        content4.fillAmount = 0;
        content5.fillAmount = 0;
        content6.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

        void OnTriggerEnter2D(Collider2D col)
        {
            switch (col.gameObject.tag)
            {
            case "TrigS1":
                print("Estoy en Sala 1");
                content1.fillAmount = 1;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                break;
            case "TrigS2":
                print("Estoy en Sala 2");
                content1.fillAmount = 0;
                content2.fillAmount = 1;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                break;
            case "TrigS3":
                print("Estoy en Sala 3");
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 1;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                break;
            case "TrigS4":
                print("Estoy en Sala 4");
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 1;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                break;
            case "TrigS5":
                print("Estoy en Sala 5");
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 1;
                content6.fillAmount = 0;
                break;
            case "TrigS6":
                print("Estoy en Sala 6");
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 1;
                break;
            }
        }
}
