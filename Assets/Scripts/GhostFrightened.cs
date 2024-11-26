using UnityEngine;

public class GhostFrightened : GhostBehavior // Hérite de la classe GhostBehavior pour définir un comportement effrayé du fantôme
{
    public SpriteRenderer body; // SpriteRenderer du corps du fantôme
    public SpriteRenderer eyes; // SpriteRenderer des yeux normaux du fantôme
    public SpriteRenderer blue; // SpriteRenderer des yeux bleus du fantôme
    public SpriteRenderer white; // SpriteRenderer des yeux blancs du fantôme

    private bool eaten; // Indique si le fantôme a été mangé

    // Méthode pour activer le comportement effrayé avec une durée spécifique
    public override void Enable(float duration)
    {
        base.Enable(duration); // Appelle la méthode Enable de la classe de base

        // Cache le corps et les yeux normaux, affiche les yeux bleus
        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        // Démarre le clignotement des yeux après la moitié de la durée
        Invoke(nameof(Flash), duration / 2f);
    }

    // Méthode pour désactiver le comportement effrayé
    public override void Disable()
    {
        base.Disable(); // Appelle la méthode Disable de la classe de base

        // Réaffiche le corps et les yeux normaux, cache les yeux bleus
        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    // Méthode appelée lorsque le fantôme est "mangé" par le Pacman
    private void Eaten()
    {
        eaten = true;
        ghost.SetPosition(ghost.home.inside.position); // Replace le fantôme à sa position de départ
        ghost.home.Enable(duration); // Active le comportement de retour à la maison du fantôme

        // Cache le corps, affiche les yeux normaux
        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    // Méthode pour effectuer le clignotement des yeux
    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false; // Cache les yeux bleus
            white.enabled = true; // Affiche les yeux blancs
            white.GetComponent<AnimatedSprite>().Restart();
        }
    }

    private void OnEnable()
    {
        blue.GetComponent<AnimatedSprite>().Restart();// Redémarre l'animation des yeux bleus
        ghost.movement.speedMultiplier = 0.5f;// Réduit la vitesse du mouvement du fantôme
        eaten = false;// Réinitialise le statut "mangé" du fantôme
    }

    private void OnDisable()
    {
        ghost.movement.speedMultiplier = 1f; // Rétablit la vitesse normale du mouvement du fantôme
        eaten = false; // Réinitialise le statut "mangé" du fantôme
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>(); // Récupère le composant Node du collider

        if (node != null && enabled) // Vérifie si le collider contient un Node et si le comportement est activé
        {
            Vector2 direction = Vector2.zero; // Direction initiale
            float maxDistance = float.MinValue; // Distance maximale initiale

            // Trouve la direction disponible qui s'éloigne le plus du Pacman
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // Si la distance dans cette direction est supérieure à la distance maximale actuelle
                // alors cette direction devient la nouvelle direction la plus éloignée
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude; // Calcule la distance au carré pour des raisons de performance

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction); // Définit la direction de déplacement du fantôme
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman")) // Vérifie si la collision est avec le Pacman
        {
            if (enabled) {
                Eaten(); // Si le comportement est activé, déclenche la méthode Eaten
            }
        }
    }
}