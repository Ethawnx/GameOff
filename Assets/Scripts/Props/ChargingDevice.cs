using UnityEngine;

public class ChargingDevice : MonoBehaviour, IInteractable
{
    Player player;
    public void OnInteract() 
    {
        player.ChargeDagger();
    }
    public void PopUI() 
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
}
