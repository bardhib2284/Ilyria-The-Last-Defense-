using UnityEngine;

public static class FromCharacterToCharacterJson
{
    public static CharacterJson ConvertTo(Character c)
    {
        Debug.Log(c);
        if(c.items == null)
        {
            return new CharacterJson { Id = c.ID, Current_Level = c.Level_Current, Items = null, Name = c.name, Stars = (int)c.Stars, Current_Experience = c.Experience_Current };
        }
        return new CharacterJson{ Id = c.ID, Current_Level = c.Level_Current, Items = c.Items, Name = c.name, Stars = (int)c.Stars, Current_Experience = c.Experience_Current };
    }

    public static Character ConvertToCharacter(CharacterJson characterJson)
    {
        GameObject gameObject = Resources.Load<GameObject>("HeroPrefabs/"+characterJson.Name);
        Character c = gameObject.GetComponent<Character>();
        Character temp = c;
        temp.Level_Current = characterJson.Current_Level;
        temp.Experience_Current = characterJson.Current_Experience;
        temp.ID = characterJson.Id;
        if(characterJson.Items != null && characterJson.Items.Count > 0)
        {
            temp.items = new System.Collections.Generic.List<Item>();
            temp.items.AddRange(characterJson.Items);
        }
        return temp;
    }
}