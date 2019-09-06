using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class RewardManager : MonoBehaviour
{
    public Reward reward;

    public static RewardManager instance;
    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    public void ReceiveRewards()
    {
        reward.ReceiveReward();
    }

    public static void ShowRewardsUI(Reward rewards,Transform rewardHolder)
    {
        foreach (var reward in rewards.consumables)
        {
            GameObject reward_ui = Instantiate(Resources.Load<GameObject>("UI/Reward_UI"), rewardHolder);
            reward_ui.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(reward.Icon);
            reward_ui.transform.GetChild(1).GetComponent<Text>().text = reward.Name;
            reward_ui.transform.GetChild(2).GetComponent<Text>().text = reward.Value.ToString();
        }
    }

    public void ShowHeroExeprience(List<Character> characters,int totalExperience, Transform characterHolder)
    {
        foreach(var c in characters)
        {
            GameObject hero_exp_ui = Instantiate(Resources.Load<GameObject>("UI/Hero_UI_Experience"), characterHolder);
            StartCoroutine(UpdateExperienceUI(c,totalExperience, hero_exp_ui.transform));
        }
    }

    public IEnumerator UpdateExperienceUI(Character character, int totalExperience, Transform Hero_Experience_UI)
    {
        Image uiExpAnimation = Hero_Experience_UI.transform.GetChild(2).GetChild(0).GetComponent<Image>();
        var text = Hero_Experience_UI.transform.GetChild(2).GetChild(1).GetComponent<Text>().text;
        text =  character.Experience_Current + "//" + character.Experience_LevelUp;
        float experienceChange = (float)character.Experience_Current / (float)character.Experience_LevelUp;
        uiExpAnimation.fillAmount = experienceChange;
        Debug.Log("Exp Required " + character.Experience_LevelUp);
        while (totalExperience > 0)
        {
            Debug.Log("Debuging in the while");
            character.Experience_Current =+ 1;
            experienceChange = (float)character.Experience_Current / (float)character.Experience_LevelUp;
            uiExpAnimation.fillAmount = experienceChange;
            if (character.Experience_Required <= 0)
            {
                character.Level_Current += 1;
            }
            text = character.Experience_Current + "//" + character.Experience_LevelUp;
            yield return new WaitForSeconds(0.15f);
            totalExperience -= 1;
        }
    }
}   