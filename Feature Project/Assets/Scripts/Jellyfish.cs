using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour
{
    public Vector3 jellyfishPos;
    public float jellyAmp;
    public float jellyFreq;
    public GameObject jellyRadius;
    public bool jellyHealing;
    public float healTime = 1f;

    public void Start()
    {
        jellyHealing = false;
        GetComponent<Collider>().enabled = false;
        jellyRadius.SetActive(false);
        jellyfishPos = transform.position;
    }
    public void Update()
    {
        transform.position = new Vector3(jellyfishPos.x, Mathf.Sin(Time.time * jellyFreq) * jellyAmp + jellyfishPos.y, 0);
        if(jellyHealing == true)
        {
            GetComponent<Collider>().enabled = true;
            jellyRadius.SetActive(true);
            StartCoroutine(JellyHealInactive());
        }
        else
        {
            GetComponent<Collider>().enabled = false;
            jellyRadius.SetActive(false);
            StartCoroutine(JellyHealActive());
        }
    }

    private IEnumerator JellyHealActive()
    {
        yield return new WaitForSeconds(healTime);
        jellyHealing = true;
    }

    private IEnumerator JellyHealInactive()
    {
        yield return new WaitForSeconds(healTime);
        jellyHealing = false;
    }
    public void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject.tag == "Player")
        {
            if (KokomiController.instance.ultimateActive)
            {
                UltHeal();
            }
            else
            {
                Heal();
            }
        }
    }
    public void Heal()
    {
        KokomiController.instance.currentHealth += KokomiController.instance.healing;
    }

    public void UltHeal()
    {
        KokomiController.instance.currentHealth += KokomiController.instance.buffedHealing;
    }

}
