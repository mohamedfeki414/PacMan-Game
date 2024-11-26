using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Exige un Rigidbody2D sur l'objet
public class Movement : MonoBehaviour
{
    public float speed = 8f; // Vitesse de déplacement par défaut
    public float speedMultiplier = 1f; // Multiplicateur de vitesse
    public Vector2 initialDirection; // Direction initiale du mouvement
    public LayerMask obstacleLayer; // Masque de collision pour les obstacles

    // Propriétés accessibles en dehors de la classe
    public new Rigidbody2D rigidbody { get; private set; } // Rigidbody2D attaché à l'objet
    public Vector2 direction { get; private set; } // Direction actuelle de déplacement
    public Vector2 nextDirection { get; private set; } // Prochaine direction de déplacement en attente
    public Vector3 startingPosition { get; private set; } // Position de départ de l'objet

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>(); // Récupère le Rigidbody2D de l'objet
        startingPosition = transform.position; // Initialise la position de départ
    }

    private void Start()
    {
        ResetState(); // Réinitialise l'état du mouvement
    }

    public void ResetState()
    {
        speedMultiplier = 1f; // Réinitialise le multiplicateur de vitesse
        direction = initialDirection; // Définit la direction initiale
        nextDirection = Vector2.zero; // Réinitialise la prochaine direction
        transform.position = startingPosition; // Replace l'objet à sa position de départ
        rigidbody.isKinematic = false; // Désactive le mode kinématique du Rigidbody
        enabled = true; // Active le composant de mouvement
    }

    private void Update()
    {
        // Essaye de se déplacer dans la prochaine direction en attente pour une réactivité accrue
        if (nextDirection != Vector2.zero) {
            SetDirection(nextDirection); // Définit la prochaine direction comme direction actuelle
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position; // Position actuelle
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime; // Calcul du déplacement

        rigidbody.MovePosition(position + translation); // Déplace l'objet en fonction de la direction et de la vitesse
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        // Définit la direction si la tuile dans cette direction est disponible
        // sinon, la définira comme prochaine direction
        if (forced || !Occupied(direction))
        {
            this.direction = direction; // Définit la direction actuelle
            nextDirection = Vector2.zero; // Réinitialise la prochaine direction
        }
        else
        {
            nextDirection = direction; // Définit la prochaine direction
        }
    }

    public bool Occupied(Vector2 direction)
    {
        // Vérifie s'il y a un obstacle dans la direction spécifiée
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
        return hit.collider != null; // Retourne vrai s'il y a un obstacle
    }

}
