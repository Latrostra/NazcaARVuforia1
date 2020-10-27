/*
amqpTest:
    - co robi:
        - wywala odebrane wiadomości na konsole
        - może wysyłać co jakiś czas numerki po amqp
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Newtonsoft.Json.Linq;

public class amqpTest : AMQPobject
{
    private int num = 1;
    [SerializeField]
    private bool sendNums;
    protected override void consume(string key, JObject body){
        var type = body.Value<string>("Flavor");
        if (type == "Bool") {
            Debug.Log($"Recived {key}: {body.Value<bool>("Value").ToString()}");
        }
        else if (type == "text") {
            Debug.Log($"Recived {key}: {body.Value<string>("Value")}");
        }
        else {
            Debug.Log($"Recived {key}: {body.Value<int>("Value").ToString()}");
        }
    }
    private void FixedUpdate() {
        if (sendNums){
            num++;
            if (num % 200 == 0){
                this.sendText(num.ToString());
                Debug.Log("Sent num="+num.ToString());
            }
        }   
    }
}
