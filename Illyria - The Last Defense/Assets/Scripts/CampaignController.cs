using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CampaignController : MonoBehaviour
{
    public bool Finished;
    public Mode Mode;
    public Reward Reward;
    public GameObject preFightObject;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        var showcasePrefab = Resources.Load<GameObject>("UI/Level_Showcase");
        GameObject showcaseTemp = Instantiate(showcasePrefab, this.transform.parent);
        showcaseTemp.GetComponentInChildren<Button>().onClick.AddListener(delegate {
            Destroy(showcaseTemp);
        });
        var enemyHolder = showcaseTemp.transform.GetChild(1);
        var rewardHolder = showcaseTemp.transform.GetChild(2);
        foreach(var enemy in Mode.Mode_Characters_Enemies)
        {
            GameObject hero_ui = Instantiate(Resources.Load<GameObject>("UI/Hero_UI"), enemyHolder);
            hero_ui.GetComponent<Animator>().enabled = false;
            hero_ui.GetComponent<Button>().interactable = false;
            hero_ui.transform.GetChild(0).GetComponent<Image>().sprite = enemy.CharacterIcon;
            hero_ui.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("FactionIcons/" + enemy.Faction.ToString());
            hero_ui.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = Mode.Mode_Characters_Levels.ToString();
            hero_ui.transform.GetChild(1).GetComponent<Text>().text = enemy.Name;
            for (int i = 0; i < (int)Mode.Mode_Character_Enemies_Stars; i++)
            {
                Instantiate(Resources.Load<GameObject>("UI/Star"), hero_ui.transform.GetChild(0).GetChild(0));
            }
        }
        RewardManager.ShowRewardsUI(Reward, rewardHolder);
        /*foreach(var reward in Reward.consumables)
        {
            GameObject reward_ui = Instantiate(Resources.Load<GameObject>("UI/Reward_UI"), rewardHolder);
            reward_ui.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(reward.Icon);
            reward_ui.transform.GetChild(1).GetComponent<Text>().text = reward.Name;
            reward_ui.transform.GetChild(2).GetComponent<Text>().text = reward.Value.ToString();
        }*/

        showcaseTemp.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate
        {
            showcaseTemp.SetActive(false);
            preFightObject.SetActive(true);
            preFightObject.GetComponent<PreFight>().OpenedFrom = Mode;
        });
        Mode.Rewards = this.Reward;
        GameManager.instance.Active_Mode = this.Mode;
    }

}