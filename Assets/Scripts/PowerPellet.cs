using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f; // Durée de l'effet de pellet de puissance en secondes

    protected override void Eat()
    {
        
        GameManager.Instance.PowerPelletEaten(this); // Appelle la méthode PowerPelletEaten dans GameManager
        
    }

}


