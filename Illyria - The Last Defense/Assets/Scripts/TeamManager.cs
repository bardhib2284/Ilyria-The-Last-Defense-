using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager instance;
    public List<Character> Characters;

    private void Start()
    {
        StartCoroutine("GetCharactersFromDatabase");
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

    public void AddANewCharacterToTheTeam(Character c)
    {
        DBTestBehaviourScript.instance.AddCharacter(c);
        StartCoroutine("GetCharactersFromDatabase");
    }

    private IEnumerator GetCharactersFromDatabase()
    {
        yield return Characters = DBTestBehaviourScript.instance.ReadCharacters();
    }

    public void UpdateCharactersExperience(List<Character> characters)
    {
        StartCoroutine("UpdateCharactersExperienceToDatabase", characters);
    }

    private IEnumerator UpdateCharactersExperienceToDatabase(List<Character> characters)
    {
        Debug.Log("Updating the player characters in the database ");
        foreach (var c in characters)
        {
            DBTestBehaviourScript.instance.UpdateCharacterExperience(c);
            yield return null;
        }
    }
}