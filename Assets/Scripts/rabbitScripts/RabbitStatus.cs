/*
RabbitStatus:
    - co robi:
        - Wyświetla status AMQP
    - czego wymaga:
        - AMQP (gdzieś w scenie)
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RabbitStatus : MonoBehaviour
{
    [Tooltip("Text gdzie będzie wyświetlany status")]
    public TextMeshProUGUI text;
    [Tooltip("w {0} zostanie wstawione Connected/Not connected")]
    public string formatText;
    private AMQP aMQP;
    private void Start() {
        aMQP = FindObjectOfType<AMQP>();        
    }
    // Every frame check if connected
    void Update()
    {
        // Set text 
        string amqpString;
        if (aMQP.isConnected()){
            amqpString = "Connected";
        }
        else {
            amqpString = "not Connected";
        }
        text.text = string.Format(formatText, amqpString);
    }
}
