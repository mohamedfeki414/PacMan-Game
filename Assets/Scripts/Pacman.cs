using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    [SerializeField]
    private AnimatedSprite deathSequence; // Séquence animée de la mort
    private SpriteRenderer spriteRenderer; // Renderer de sprite
    private Movement movement; // Composant de mouvement
    private new Collider2D collider; // Collider

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Récupère le SpriteRenderer
        movement = GetComponent<Movement>(); // Récupère le composant de mouvement
        collider = GetComponent<Collider2D>();
    }
private void Update()
{
    // Définit la nouvelle direction en fonction de l'entrée actuelle
    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
        movement.SetDirection(Vector2.up);
    }
    else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
        movement.SetDirection(Vector2.down);
    }
    else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
        movement.SetDirection(Vector2.left);
    }
    else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
        movement.SetDirection(Vector2.right);
    }

    // Fait tourner Pacman pour qu'il fasse face à la direction de mouvement
    float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
    transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
}

public void ResetState()
{
    enabled = true;
    spriteRenderer.enabled = true;
    collider.enabled = true;
    deathSequence.enabled = false;
    movement.ResetState();
    gameObject.SetActive(true);
}

public void DeathSequence()
{
    enabled = false;
    spriteRenderer.enabled = false;
    collider.enabled = false;
    movement.enabled = false;
    deathSequence.enabled = true;
    deathSequence.Restart();
}


}
