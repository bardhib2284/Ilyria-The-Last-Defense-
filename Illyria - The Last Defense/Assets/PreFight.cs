using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreFight : MonoBehaviour
{
    public Mode OpenedFrom;
    public List<Character> Characters;
    public List<CharacterJson> CharacterJsons;
    // Start is called before the first frame update
    void Start()
    {
        Characters = new List<Character>();
        CharacterJsons = new List<CharacterJson>();
        TeamManager team = FindObjectOfType<TeamManager>();
        foreach(var character in team.Characters)
        {
            Debug.Log("Current Character : " + character);
            GameObject hero_ui = Instantiate(Resources.Load<GameObject>("UI/Hero_UI_Dragable"),this.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0));
            hero_ui.GetComponent<Animator>().enabled = false;
            hero_ui.GetComponent<Button>().interactable = true;
            hero_ui.transform.GetChild(0).GetComponent<Image>().sprite = character.CharacterIcon;
            hero_ui.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("FactionIcons/" + character.Faction.ToString());
            hero_ui.transform.GetChild(1).GetComponent<Text>().text = character.Name;
            hero_ui.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = character.Level_Current.ToString();

            for (int i = 0; i < (int)character.Stars; i++)
            {
                Instantiate(Resources.Load<GameObject>("UI/Star"), hero_ui.transform.GetChild(0).GetChild(0));
            }

            hero_ui.GetComponent<Dragable>().character = character;
        }
        transform.GetChild(2).GetComponent<Button>().onClick.AddListener(
        delegate
        {
            Characters.Clear();
            DropZone[] dragablesSelected = GetComponentsInChildren<DropZone>();
            for (int i = 0; i < dragablesSelected.Length; i++)
            {
                if(dragablesSelected[i].transform.childCount > 0)
                {
                    if (dragablesSelected[i].transform.GetChild(0).GetComponent<Dragable>())
                    {
                        Character character = dragablesSelected[i].transform.GetChild(0).GetComponent<Dragable>().character;
                        Characters.Add(character);
                        CharacterJsons.Add(FromCharacterToCharacterJson.ConvertTo(character));
                    }
                }
            }
            if(Characters.Count > 0)
            {
                GameManager.instance.Active_Mode = OpenedFrom;
                //GameManager.instance.playerCharacters = Characters;
                GameManager.instance.playerCharacterJsons = CharacterJsons;
                LevelManager.instance.GoToFightScene();
            }
            else
            {
                Dialog.instance.CreateAlertDialog("Please Select One Of Your Characters", "Ok");
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
}