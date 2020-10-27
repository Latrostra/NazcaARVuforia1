using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAMQP : MonoBehaviour, IClickAction
{
    private AMQPobject amqp;
    private AMQP connection;

    private void Start() {
        amqp = GetComponent<AMQPobject>();
        connection = FindObjectOfType<AMQP>();
    }

    public void ClickAction() {
        if (connection.isConnected()) {
            amqp.sendBool(true);
        }
    }
}
