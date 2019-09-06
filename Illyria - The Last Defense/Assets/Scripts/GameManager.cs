using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int round = 0;
    //TODO : THIS IS UGLY AS SHIT , FIX AS SOON AS POSSIBLE
    // FIX : SEND THE CHARACTERS THROUGH ANOTHER SCRIPT FOR THE PLAYER AND GET THE CHARACTERS FOR THE ENEMY FROM THE CAMPAIGNS
    [Space]
    [Header("All Characters PROPERTIES")]
    public List<Character> playerCharacters;
    public List<CharacterJson> playerCharacterJsons;
    [Header("MODE PROPERTIES")]
    [Tooltip("Mode : Everything that is fightable is a mode")]
    public Mode Active_Mode;
    //TODO: THIS IS USED ONLY FOR GRAPHICAL ISSUES, DELETE AND USE THE ACTIVE MODE ENEMIES
    public List<Character> modeCharacters;
    [Space]
    [Header("All Characters PROPERTIES")]
    [Tooltip("This Array Is Used For Getting All The Characters And Sorting Based On Speed Since Speed Prioritizes Attack")]
    [SerializeField]
    private List<Character> allCharacters;
    public List<GameObject> instantiatedGameObjects;
    public bool GameFinished;

    [Space]
    public GameObject WinUI;
    public GameObject LoseUI;

    private void Awake()
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
        Application.targetFrameRate = 300;

    }
    private void Start()
    {
        //Init();
        //Invoke("SaveCharacters",0f);
    }

    //ROUND LOGIC
    public IEnumerator StartRound()
    {
        while (round < 15)
        {
            if (GameFinished)
            {
                break;
            }
            Debug.Log("ROUND " + round + " STARTED WITH : " + allCharacters.Count + " CHARACTERS LEFT");
            foreach (var character in allCharacters)
            {
                if (!character.Dead && !character.Stunned)
                {
                    if (character.tag == "Left")
                    {
                        character.enemyTeam = allCharacters.FindAll((Character obj) => obj.gameObject.tag == "Right");
                        character.enemyTeam = new List<Character>(character.enemyTeam.FindAll((Character obj) => obj.Dead == false));
                        if (character.enemyTeam.Count < 1)
                        {
                            Win();
                            break;
                        }
                        Debug.LogError("DWDWDWDWDWDWD");
                        character.StartTurn();
                        yield return new WaitUntil(() => character.IsBusy == false);
                    }
                    else if (character.tag == "Right")
                    {
                        character.enemyTeam = allCharacters.FindAll((Character obj) => obj.gameObject.tag == "Left");
                        character.enemyTeam = new List<Character>(character.enemyTeam.FindAll((Character obj) => obj.Dead == false));
                        if (character.enemyTeam.Count < 1)
                        {
                            Lose();
                            break;
                        }
                        Debug.LogError("DWDWDWDWDWDWD");
                        character.StartTurn();
                        yield return new WaitUntil(() => character.IsBusy == false);

                        //yield return new WaitUntil(() => character.IsBusy = false);
                    }
                }
            }

            allCharacters = allCharacters.FindAll((i) => i.GetDead() == false);
            round++;
        }
        //Active_Mode.Rewards.ReceiveReward();
    }

    private void Lose()
    {
        Debug.Log("Lose");
        GameFinished = true;
    }

    private void Win()
    {
        Debug.Log("WIN");
        GameFinished = true;
        var mainCanvas = FindObjectsOfType<Canvas>().First(x => x.name == "Canvas");
        GameObject VictoryPanelClone = Instantiate(WinUI, mainCanvas.transform);
        RewardManager.ShowRewardsUI(Active_Mode.Rewards, VictoryPanelClone.transform.GetChild(1));
        VictoryPanelClone.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate
        {
            LevelManager.instance.GoToHomeScene();
        });
        Active_Mode.Rewards.ReceiveReward();
        RewardManager.instance.ShowHeroExeprience(playerCharacters,Active_Mode.Experience,VictoryPanelClone.transform.GetChild(2));
        //int experienceEarned = Active_Mode.Experience / playerCharacters.Count >= 1 ? playerCharacters.Count : (playerCharacters.Count + 1);
        //int experienceEarned = Active_Mode.Experience / playerCharacters.Count ;

        /*for (int i = 0; i < playerCharacters.Count; i++)
        {
            Debug.Log("Current Player Character : " + playerCharacters[i].Name + " has gained " + experienceEarned + " his old experience is : " + playerCharacters[i].Experience_Current);
            playerCharacters[i].Experience_Current += experienceEarned;
            Debug.Log("Current Player Character : " + playerCharacters[i].Name + " has gained " + experienceEarned + " his current experience is : " + playerCharacters[i].Experience_Current);
        }*/
        //TeamManager.instance.UpdateCharactersExperience(playerCharacters);
    }

    public void Init()
    {
        Time.timeScale = 2f;
        allCharacters = new List<Character>();
        instantiatedGameObjects = new List<GameObject>();
        if(Active_Mode != null)
            modeCharacters = Active_Mode.Mode_Characters_Enemies;
        Transform holder = FindObjectOfType<PositionHolder>().transform;
        int index = 0;
        foreach (var c in playerCharacterJsons)
        {
            Debug.Log(c + " game manager debug");
            if (holder != null)
            {
                Debug.Log("HeroPrefabs/" + c.Name);
                Debug.Log(c.Current_Experience + " Experience ");
                c.Name = c.Name.Split('(')[0];
                GameObject temp = Instantiate(Resources.Load<GameObject>("HeroPrefabs/" + c.Name), holder.GetChild(0).GetChild(index++));
                Character character = temp.GetComponent<Character>();
                character.ID = c.Id;
                character.Level_Current = c.Current_Level;
                character.Experience_Current = c.Current_Experience;
                character.Stars = (Character.CharacterStars)c.Stars;
                character.UpdateLevelStats();
                instantiatedGameObjects.Add(temp);
                temp.tag = "Left";
                allCharacters.Add(character);
                playerCharacters.Add(character);
            }
        }
        index = 0;
        foreach (var c in modeCharacters)
        {
            if (holder != null)
            {
                GameObject temp = Instantiate(Resources.Load<GameObject>("HeroPrefabs/" + c.name), holder.GetChild(1).GetChild(index++));
                Character character = temp.GetComponent<Character>();
                character.UpdateLevelStats();
                instantiatedGameObjects.Add(temp);
                temp.tag = "Right";
                character.Level_Current = Active_Mode.Mode_Characters_Levels;
                allCharacters.Add(temp.GetComponent<Character>());
            }
        }
        Debug.Log(allCharacters.Count);
        instantiatedGameObjects = new List<GameObject>(instantiatedGameObjects.OrderByDescending(i => i.GetComponent<Character>().Speed_Max));
        allCharacters = new List<Character>(allCharacters.OrderByDescending(i => i.Speed_Max));
        StartCoroutine("StartRound");
    }

    public async void SaveCharacters()
    {
        //await DatabaseManager.WriteToCharacters(JsonConvert.SerializeObject(modeCharacters));
    }
}