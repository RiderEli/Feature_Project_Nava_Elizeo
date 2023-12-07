using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* [Nava, Elizeo]
 * [December 7, 2023]
 * [This is the code for the Attack projectile.]
 */
public class AttackFish : MonoBehaviour
{
    public float fishSpeed = 10f;
    private Rigidbody fishRB;
    private void Awake()
    {
        fishRB = GetComponent<Rigidbody>();
        fishRB.velocity = transform.forward * fishSpeed;
    }
    private void Update()
    {
        if (KokomiController.instance.atkHealActive)
        {
            StartCoroutine(ATKHealTime());
            KokomiController.instance.attackHealingText.SetActive(true);
        }
        else
        {
            KokomiController.instance.attackHealingText.SetActive(false);
        }
    }
    public IEnumerator ATKHealTime()
    {
        float healTime = 0.3f;
        yield return new WaitForSeconds(healTime);
        KokomiController.instance.atkHealActive = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "I-Wall")
        {
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
            //This is the attack healing effect that will be active if Kokomi activates her Ult/Elemental Skill.
            if (KokomiController.instance.ultimateActive)
            {
                KokomiController.instance.currentHealth += KokomiController.instance.attackHealing;
                KokomiController.instance.atkHealActive = true;
            }
        }
    }

}
