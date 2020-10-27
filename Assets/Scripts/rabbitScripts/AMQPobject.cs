/*
AMQPobject:
    - Co robi:
        - wysyła wiadomości do Nazci na dany klucz
        - odbiera wiadomości z danego klucza przez callback
    - Na czym powinien być:
        - na przyciskach itp co mają wysyłać coś do Nazci
        - rozszerzać w celu odbierania wiadomości
    - inne:
        - wymaga gdzieś komponentu AMQP w scenie

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AMQPobject : MonoBehaviour
{
    public bool rateLimited;
    private bool canSend = true;
    public float sendOnceSeconds;
    public IModel model;
    private string exchangeName = "InteropServices";
    [SerializeField, Tooltip("Klucz na który będą wysyłane wiadomości")]
    private string keyName;
    [SerializeField, Tooltip("Czy obiekt powinien łączyć się przy starcie")]
    private bool autoStart;

    public string senderName(){
        //Zwraca string (model urządzenia; gameObject; ARnazca)
        return SystemInfo.deviceModel + "; " + gameObject.name + "; ARnazca";
    }
    public void reconnect(){
        connect(exchangeName, keyName);
    }

    void Start()
    {
        if (autoStart)
        {
            connect(exchangeName, keyName);
        }
    }

    public void connect(string exchangeName_, string keyName_)
    {
        // pobiera model od obiektu AMQP do komunikacji z Nazca, ustawia go
        exchangeName = exchangeName_;
        keyName = keyName_;
        model = FindObjectOfType<AMQP>().GetModel(this);   
        model.ExchangeDeclare(exchangeName, "direct");
        var queueName = model.QueueDeclare();
        model.QueueBind(queueName.QueueName, exchangeName, keyName);
        var consumer = new EventingBasicConsumer(model);
        consumer.Received += (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            deserialize(ea.RoutingKey, body);
        };
        model.BasicConsume(queueName.QueueName, true, consumer);
        
    }
    public void sendBool(bool value){
        // Ponieważ apple jest upośledzone i wymaga równie upośledzonych metod
        #if UNITY_IOS
            var val = value ? "true" : "false";
            var msgstr = "{\"Flavor\": \"Bool\", " +
            "\"Value\": " + val + ", " +
            "\"Sender\": \"" + "iPhone;ARNazca" + "\" }";
            var message = Encoding.UTF8.GetBytes(msgstr);
        #else
            var jsonObj = new JObject();
            jsonObj.Add("Flavor", "Bool");
            jsonObj.Add("Value", value);
            jsonObj.Add("Sender", senderName());
            var message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jsonObj));
        #endif
        sendBytes(message);
    }
    public void sendText(string value){
        #if UNITY_IOS
            var msgstr = "{\"Flavor\": \"Text\", " +
            "\"Value\": \"" + value + "\", " +
            "\"Sender\": \"" + "iPhone;ARNazca" + "\" }";
            var message = Encoding.UTF8.GetBytes(msgstr);
        #else
            var jsonObj = new JObject();
            jsonObj.Add("Flavor", "Bool");
            jsonObj.Add("Value", value);
            jsonObj.Add("Sender", senderName());
            var message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jsonObj));
        #endif
        sendBytes(message);
    }
    public void sendInt(int value){
        #if UNITY_IOS
            var val = value.ToString();
            var msgstr = "{\"Flavor\": \"Num\", " +
            "\"Value\": " + val + ", " +
            "\"Sender\": \"" + "iPhone;ARNazca" + "\" }";
            var message = Encoding.UTF8.GetBytes(msgstr);
        #else
            var jsonObj = new JObject();
            jsonObj.Add("Flavor", "Num");
            jsonObj.Add("Value", value);
            jsonObj.Add("Sender", senderName());
            var message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jsonObj));
        #endif
        sendBytes(message);
    }

    private void sendBytes(byte[] message){
        // wysyła wiadomość zakodowaną jako json przez metody send *
        if (canSend || !rateLimited) {
            var props = model.CreateBasicProperties();
            Debug.Log("Sending amqp from " + gameObject.name);
            props.ContentType = "text/plain";
            props.DeliveryMode = 2;
            model.BasicPublish(exchangeName, keyName, false, props, message);
            // Rozpoczyna korutynę zmieniającą canSend na parę sekund
            StartCoroutine(rateLimit());
        }
        else if (!canSend && rateLimited) {
            Debug.LogWarning("RateLimit in " + gameObject.name);
        }
    }
    private void deserialize(string key, byte[] body){
        var json = JsonConvert.DeserializeObject<JObject>(Encoding.UTF8.GetString(body));
        consume(key, json);
    }
    protected virtual void consume(string key, JObject body){}
    private void OnDestroy() {
        if (model != null) {
            model.Dispose();
        }
    }
    private IEnumerator rateLimit(){
        // Zmienia canSend na parę sekund
        if (rateLimited) {
            canSend = false;
            yield return new WaitForSeconds(sendOnceSeconds);
            canSend = true;
        }
    }
}
