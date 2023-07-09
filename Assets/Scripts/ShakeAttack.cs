using System.Collections;
using UnityEngine;

public class ShakeAttack : MonoBehaviour
{

    // Update is called once per frame
    float duration = 0.5f; //how fast it shakes
    float speed = 80f; //how much it shakes

    IEnumerator Shake()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        
        while(elapsedTime< duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = startPosition + new Vector3(0.1f*Mathf.Sin(elapsedTime * speed), 0.05f*Mathf.Sin(elapsedTime * speed),0);
            yield return null;

        }

        transform.position = startPosition;
    }
    
    void OnEnable()
    {
        StartCoroutine(Shake());
    }



}

