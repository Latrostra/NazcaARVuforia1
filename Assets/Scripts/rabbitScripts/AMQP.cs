/*
AMQP:
    - Co robi:
        - Zarząda połączeniem z Nazca
        - zarząda model-ami w AMQPobject
    - Na czym powinien być:
        - Na kamerze lub innym czymś co nie zniknie
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

public class AMQP : MonoBehaviour
{
    [SerializeField, Tooltip("RabbitMQ server url with password, login and port")]
    private string uri_string;
    private ConnectionFactory connectionFactory;
    private IConnection connection;

    // List of AMQPObjects that have requested a model
    private List<AMQPobject> clients = new List<AMQPobject>();

    void Awake() 
    {
        // Połącz sie z rabbitem
        connectionFactory = new ConnectionFactory()
        {
            Uri = new Uri(uri_string)
        };
        connection = connectionFactory.CreateConnection();
        Debug.Log("Connected with RabbitMQ");
        // If connection dies -> try to reconnect
        StartCoroutine(keepConnectionAlive());
    }
    public bool isConnected(){
        return connection.IsOpen;
    }

    public void forceReconnect(string new_uri = null){
        // pozwala na zmianę uri AMQP
        if (new_uri != null){
            uri_string = new_uri;
            connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(uri_string)
            };
            connection = connectionFactory.CreateConnection();
            Debug.Log("Connected with RabbitMQ");
        }
        Debug.LogWarning("Reconnecting to rabbit");
        connection = connectionFactory.CreateConnection();
        var newClients = new List<AMQPobject>(clients);
        foreach (AMQPobject client in newClients) {
            try {
                // Rozdaj nowe modele
                client.model = connection.CreateModel();
                client.reconnect();
            }
            catch {

            }
        }
        clients = newClients;
    }

    public IEnumerator keepConnectionAlive(){
        // Korutyna sprawdzająca czy połączenie nadal działa, jeśli nie to łączy ponownie
        while (true) {
            if (!isConnected()){
                forceReconnect();
            }
            yield return new WaitForSeconds(5);
        }
    }
    public IModel GetModel(AMQPobject client) {
        // Zwraca model dla AMQPobject, wpisuje go na listę klientów
        clients.Add(client);
        var model = connection.CreateModel();
        return model;
    }
    private void OnDestroy() {
        foreach (AMQPobject client in clients)
        {
            try
            {
                client.model.Dispose();
            }
            catch
            {}
        }
        connection.Dispose();
    }
    public string getServerURI(){
        return uri_string;
    }

    private void OnApplicationPause(bool pauseStatus) {
        // połącz ponownie na powrocie z tła
        if (!pauseStatus){
            forceReconnect();
        }
    }
}
