using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
/* [Nava, Elizeo]
 * [December 7, 2023]
 * [This is a UI script for the KokomiContainer object. Some of these codes using texts comes from Singletons in the KokomiController script]
 */
public class KokomiHPUI : MonoBehaviour
{
    //General Codes
    public Text healthText;
    public Text skillCooldownText;
    public Text ultCooldownText;
    public GameObject SkillCooldownUI;
    public GameObject SkillActiveUI;
    public GameObject UltCooldownUI;
    public GameObject UltActiveUI;
    public float skillTimer = 20;
    public float ultTimer = 10;
    public Jellyfish jelly;

    public void Start()
    {
        skillCooldownText.text = " ";
        ultCooldownText.text = " ";
        SkillCooldownUI.SetActive(false);
        SkillActiveUI.SetActive(false);
        UltCooldownUI.SetActive(false);
        UltActiveUI.SetActive(false);
    }
    public void Update()
    {
        healthText.text = "HP: " + KokomiController.instance.currentHealth.ToString() + "/" + KokomiController.instance.maxHealth.ToString();
        SkillCooldown();
        SkillActive();
        UltCooldown();
        UltActive();
    }

    //These codes will make the UI display a timer and an indicator of whether a skill or ult/burst is activated or not.
    public void SkillActive()
    {
        if (KokomiController.instance.skillActive)
        {
            SkillActiveUI.SetActive(true);
        }
        else
        {
            SkillActiveUI.SetActive(false);
        }
    }

    public void SkillCooldown()
    {
        if (KokomiController.instance.skillCooldown)
        {
            SkillCooldownUI.SetActive(true);
            skillCooldownText.text = skillTimer.ToString();
            skillTimer -= Time.deltaTime;
        }
        else
        {
            SkillCooldownUI.SetActive(false);
            skillTimer = 20;
            skillCooldownText.text = " ";
        }
    }
    public void UltActive()
    {
        if (KokomiController.instance.ultimateActive)
        {
            UltActiveUI.SetActive(true);
        }
        else
        {
            UltActiveUI.SetActive(false);
        }
    }
    public void UltCooldown()
    {
        if (KokomiController.instance.ultimateCooldown)
        {
            UltCooldownUI.SetActive(true);
            ultCooldownText.text = ultTimer.ToString();
            ultTimer -= Time.deltaTime;
        }
        else
        {
            UltCooldownUI.SetActive(false);
            ultTimer = 10;
            ultCooldownText.text = " ";
        }
    }
}