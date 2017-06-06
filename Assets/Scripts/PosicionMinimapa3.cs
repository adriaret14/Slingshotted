using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PosicionMinimapa3 : MonoBehaviour
{
    [SerializeField]
    private float fillAmount;

    [SerializeField]
    private Image content1, content2, content3, content4, content5, content6, content7, content8, content9, content10, content11, content12, content13;

    // Use this for initialization
    void Start()
    {
        content1.fillAmount = 0;
        content2.fillAmount = 0;
        content3.fillAmount = 0;
        content4.fillAmount = 0;
        content5.fillAmount = 0;
        content6.fillAmount = 0;
        content7.fillAmount = 0;
        content8.fillAmount = 0;
        content9.fillAmount = 0;
        content10.fillAmount = 0;
        content11.fillAmount = 0;
        content12.fillAmount = 0;
        content13.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "StartRoom":
                content1.fillAmount = 1;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                content7.fillAmount = 0;
                content8.fillAmount = 0;
                content9.fillAmount = 0;
                content10.fillAmount = 0;
                content11.fillAmount = 0;
                content12.fillAmount = 0;
                content13.fillAmount = 0;
                break;
            case "Top_Room_1":
                content1.fillAmount = 0;
                content2.fillAmount = 1;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                content7.fillAmount = 0;
                content8.fillAmount = 0;
                content9.fillAmount = 0;
                content10.fillAmount = 0;
                content11.fillAmount = 0;
                content12.fillAmount = 0;
                content13.fillAmount = 0;
                break;
            case "Top_Room_2":
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 1;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                content7.fillAmount = 0;
                content8.fillAmount = 0;
                content9.fillAmount = 0;
                content10.fillAmount = 0;
                content11.fillAmount = 0;
                content12.fillAmount = 0;
                content13.fillAmount = 0;
                break;
            case "Top_Room3":
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 1;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                content7.fillAmount = 0;
                content8.fillAmount = 0;
                content9.fillAmount = 0;
                content10.fillAmount = 0;
                content11.fillAmount = 0;
                content12.fillAmount = 0;
                content13.fillAmount = 0;
                break;
            case "Bot_Room_1":
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 1;
                content6.fillAmount = 0;
                content7.fillAmount = 0;
                content8.fillAmount = 0;
                content9.fillAmount = 0;
                content10.fillAmount = 0;
                content11.fillAmount = 0;
                content12.fillAmount = 0;
                content13.fillAmount = 0;
                break;
            case "Bot_Room_2":
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 1;
                content7.fillAmount = 0;
                content8.fillAmount = 0;
                content9.fillAmount = 0;
                content10.fillAmount = 0;
                content11.fillAmount = 0;
                content12.fillAmount = 0;
                content13.fillAmount = 0;
                break;
            case "Bot_Room3":
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                content7.fillAmount = 1;
                content8.fillAmount = 0;
                content9.fillAmount = 0;
                content10.fillAmount = 0;
                content11.fillAmount = 0;
                content12.fillAmount = 0;
                content13.fillAmount = 0;
                break;
            case "Bot_Room4":
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                content7.fillAmount = 0;
                content8.fillAmount = 1;
                content9.fillAmount = 0;
                content10.fillAmount = 0;
                content11.fillAmount = 0;
                content12.fillAmount = 0;
                content13.fillAmount = 0;
                break;
            case "Hallway":
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                content7.fillAmount = 0;
                content8.fillAmount = 0;
                content9.fillAmount = 1;
                content10.fillAmount = 0;
                content11.fillAmount = 0;
                content12.fillAmount = 0;
                content13.fillAmount = 0;
                break;
            case "Bot_Room5":
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                content7.fillAmount = 0;
                content8.fillAmount = 0;
                content9.fillAmount = 0;
                content10.fillAmount = 1;
                content11.fillAmount = 0;
                content12.fillAmount = 0;
                content13.fillAmount = 0;
                break;
            case "Bot_Room6":
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                content7.fillAmount = 0;
                content8.fillAmount = 0;
                content9.fillAmount = 0;
                content10.fillAmount = 1;
                content11.fillAmount = 0;
                content12.fillAmount = 0;
                content13.fillAmount = 0;
                break;
            case "Bot_Room7":
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                content7.fillAmount = 0;
                content8.fillAmount = 0;
                content9.fillAmount = 0;
                content10.fillAmount = 0;
                content11.fillAmount = 1;
                content12.fillAmount = 0;
                content13.fillAmount = 0;
                break;
            case "Top_Room4":
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                content7.fillAmount = 0;
                content8.fillAmount = 0;
                content9.fillAmount = 0;
                content10.fillAmount = 0;
                content11.fillAmount = 0;
                content12.fillAmount = 1;
                content13.fillAmount = 0;
                break;
            case "Top_Room5":
                content1.fillAmount = 0;
                content2.fillAmount = 0;
                content3.fillAmount = 0;
                content4.fillAmount = 0;
                content5.fillAmount = 0;
                content6.fillAmount = 0;
                content7.fillAmount = 0;
                content8.fillAmount = 0;
                content9.fillAmount = 0;
                content10.fillAmount = 0;
                content11.fillAmount = 0;
                content12.fillAmount = 0;
                content13.fillAmount = 1;
                break;
        }
    }
}
