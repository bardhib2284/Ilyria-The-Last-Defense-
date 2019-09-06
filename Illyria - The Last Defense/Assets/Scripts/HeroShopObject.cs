using UnityEngine;
using System.Collections;

public class HeroShopObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        FindObjectOfType<ShopManager>().TurnOnTheHeroSummoning();
    }
}