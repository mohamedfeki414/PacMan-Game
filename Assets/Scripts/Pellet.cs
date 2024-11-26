using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pellet : MonoBehaviour
{
    public int points = 10;// Points attribués au pellet

    protected virtual void Eat()
    {
        GameManager.Instance.PelletEaten(this);// Appelle la méthode PelletEaten dans GameManager
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman")) {
            Eat();// Appelle la méthode Manger lorsque le pellet entre en collision avec Pacman
        }
    }

}
