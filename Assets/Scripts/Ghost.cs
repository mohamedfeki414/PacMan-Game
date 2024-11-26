using UnityEngine;

[DefaultExecutionOrder(-10)] // Ordre d'exécution par défaut pour contrôler le comportement des fantômes
[RequireComponent(typeof(Movement))] // Assure que le composant Movement est attaché à l'objet
public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; } // Référence au composant Movement pour le déplacement du fantôme
    public GhostHome home { get; private set; } // Référence au point de départ du fantôme
    public GhostScatter scatter { get; private set; } // Référence au comportement de dispersion du fantôme
    public GhostChase chase { get; private set; } // Référence au comportement de poursuite du fantôme
    public GhostFrightened frightened { get; private set; } // Référence au comportement effrayé du fantôme
    public GhostBehavior initialBehavior; // Comportement initial du fantôme
    public Transform target; // Transform de la cible du fantôme
    public int points = 200; // Points attribués lorsque le fantôme est mangé

    private void Awake()
    {
        // Initialise les références aux différents composants du fantôme
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        ResetState(); // Démarre le jeu en réinitialisant l'état du fantôme
    }

    public void ResetState()
    {
        gameObject.SetActive(true); // Active le fantôme
        movement.ResetState(); // Réinitialise le mouvement du fantôme

        frightened.Disable(); // Désactive le comportement effrayé du fantôme
        chase.Disable(); // Désactive le comportement de poursuite du fantôme
        scatter.Enable(); // Active le comportement de dispersion du fantôme

        if (home != initialBehavior) {
            home.Disable(); // Désactive le point de départ du fantôme si ce n'est pas son comportement initial
        }

        if (initialBehavior != null) {
            initialBehavior.Enable(); // Active le comportement initial du fantôme
        }
    }

    public void SetPosition(Vector3 position)
    {
        // Garde la même position z pour déterminer la profondeur de dessin
        position.z = transform.position.z;
        transform.position = position; // Définit la position du fantôme
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled) {
                GameManager.Instance.GhostEaten(this); // Si le fantôme est effrayé, il est mangé par le joueur
            } else {
                GameManager.Instance.PacmanEaten(); // Sinon, le joueur est mangé par le fantôme
            }
        }
    }

}
