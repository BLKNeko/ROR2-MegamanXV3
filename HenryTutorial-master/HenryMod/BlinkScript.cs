using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{

    public GameObject blinkObject;
    public GameObject blinkObject2;
    public float timeBetweenBlink = 2f;
    private float initialStoreTime;
    // Start is called before the first frame update
    void Start()
    {

        if(blinkObject != null){
            blinkObject.transform.localScale = new Vector3(0,0,0);
            blinkObject2.transform.localScale = new Vector3(0,0,0);
        }
            

        initialStoreTime = timeBetweenBlink;
        
    }

    // Update is called once per frame
    void Update()
    {
        timeBetweenBlink -= Time.deltaTime;

        if (timeBetweenBlink <= 0f)
        {
            if(blinkObject != null){
            blinkObject.transform.localScale = new Vector3(1,1,1);
            blinkObject2.transform.localScale = new Vector3(1,1,1);
        }

            StartCoroutine("ResetBlink");
        }
        
    }

    IEnumerator ResetBlink()
    {
        yield return new WaitForSeconds(0.15f);

        if(blinkObject != null){
            blinkObject.transform.localScale = new Vector3(0,0,0);
            blinkObject2.transform.localScale = new Vector3(0,0,0);
        }

        timeBetweenBlink = initialStoreTime - (Random.Range(-1f, 1f));
    }
}
