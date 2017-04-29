using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FATEESSENCE { LIFE, DEATH, NONE };

public class FateEssenceClass : MonoBehaviour {

    private PlayerClass jugador;

    private FATEESSENCE essenceType;
    private float healthValue;

    private float DeathChance;
    //Chance of DeathEssence at max health (100)
    private float DeathMaxChance = 70.0f;
    //Chance of DeathEssence at min health (1)
    private float DeathMinChance = 5.0f;

    private float LifeChance;
    //Chance of LifeEssence at max health (100)
    private float LifeMaxChance = 0.0f;
    //Chance of LifeEssence at min health (1)
    private float LifeMinChance = 95.0f;

	// Use this for initialization
	void Start () {
        jugador = GameObject.Find("Jugador").GetComponent<PlayerClass>();
        DeathChance = DeathMinChance + (DeathMaxChance - DeathMinChance) * ((jugador.healthPoints - 1.0f) / (jugador.maxHealthPoints - 1.0f));
        LifeChance = LifeMinChance + (LifeMaxChance - LifeMinChance) * ((jugador.healthPoints - 1.0f) / (jugador.maxHealthPoints - 1.0f));

        float randomNumber = Random.Range(0.0f, 100.0f);

        if (randomNumber > DeathChance + LifeChance)
            essenceType = FATEESSENCE.NONE;
        else if (randomNumber > DeathChance)
            essenceType = FATEESSENCE.LIFE;
        else
            essenceType = FATEESSENCE.DEATH;
        
        if (essenceType != FATEESSENCE.NONE)
        {
            healthValue = Random.Range(0.0f, jugador.maxHealthPoints*0.2f);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
