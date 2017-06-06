using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{

    public GameObject player;
    public Image barra, barra2;
    public float stamina;
    public float maxStamina;
    public bool needStamina = false;

    public bool getStamina()
    {
        return (stamina >= maxStamina);
    }

    public void useStamina()
    {
        barra.rectTransform.localScale += new Vector3(-1, 0, 0);
        stamina = 0;
    }
    public void chargeStamina(float energy)
    {
        barra.rectTransform.localScale += new Vector3(energy, 0, 0);
    }
    public void updateHealth(float dmg, int flag)
    {
        
        print(dmg);
        if(flag==1)
        {
            if (player.GetComponent<PlayerClass>().healthPoints - dmg <= 0)
            {
                
                barra2.rectTransform.localScale = new Vector3(0,0,0);
            }
            else
            {
                barra2.rectTransform.localScale += new Vector3(-(dmg / 100f), 0, 0);
            }
           
        }
    }

    void Start()
    {

    }
    void Update()
    {
        if (stamina < maxStamina)
        {
            //print("hola");
            stamina = stamina + 0.1F;
            chargeStamina(0.01F);
        }
    }
}