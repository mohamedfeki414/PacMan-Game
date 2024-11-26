using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))] // Assure que le composant SpriteRenderer est attaché à l'objet
public class GhostEyes : MonoBehaviour
{
    public Sprite up; // Sprite pour les yeux vers le haut
    public Sprite down; // Sprite pour les yeux vers le bas
    public Sprite left; // Sprite pour les yeux vers la gauche
    public Sprite right; // Sprite pour les yeux vers la droite

    private SpriteRenderer spriteRenderer; // Référence au composant SpriteRenderer
    private Movement movement; // Référence au composant Movement du parent

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Initialise la référence au composant SpriteRenderer
        movement = GetComponentInParent<Movement>(); // Initialise la référence au composant Movement du parent
    }

    private void Update()
    {
        // Détermine le sprite à afficher en fonction de la direction de déplacement
        if (movement.direction == Vector2.up) {
            spriteRenderer.sprite = up; // Affiche le sprite des yeux vers le haut
        }
        else if (movement.direction == Vector2.down) {
            spriteRenderer.sprite = down; // Affiche le sprite des yeux vers le bas
        }
        else if (movement.direction == Vector2.left) {
            spriteRenderer.sprite = left; // Affiche le sprite des yeux vers la gauche
        }
        else if (movement.direction == Vector2.right) {
            spriteRenderer.sprite = right; // Affiche le sprite des yeux vers la droite
        }
    }

}

