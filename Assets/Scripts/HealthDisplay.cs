using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{

    // only the player has the PlayerHealthManager script
    private PlayerHealthManager thePlayer;
    private int healthToDisplay;

    Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        thePlayer = FindObjectOfType<PlayerHealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
       
        healthToDisplay = thePlayer.getHealth();
        if (healthToDisplay > 70)
            text.color = Color.green;
        if (healthToDisplay > 40 && healthToDisplay < 70)
            text.color = Color.yellow;
        if (healthToDisplay < 40)
            text.color = Color.red;

        text.text = "Health: " + healthToDisplay;

    }

}
