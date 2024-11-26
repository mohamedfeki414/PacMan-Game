using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehavior // Hérite de GhostBehavior pour définir le comportement du fantôme à la maison
{
    public Transform inside; // Position intérieure de la maison du fantôme
    public Transform outside; // Position extérieure de la maison du fantôme

    // Appelée lorsque le comportement est activé
    private void OnEnable()
    {
        StopAllCoroutines(); // Arrête toutes les coroutines pour éviter les transitions simultanées
    }

    // Appelée lorsque le comportement est désactivé
    private void OnDisable()
    {
        // Vérifie si l'objet est toujours actif pour éviter les erreurs lorsqu'il est détruit
        if (gameObject.activeInHierarchy) {
            StartCoroutine(ExitTransition()); // Lance la coroutine de transition de sortie
        }
    }

    // Appelée lorsque le fantôme entre en collision avec un obstacle
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Inverse la direction lorsque le fantôme heurte un mur pour créer un effet de rebondissement
        if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            ghost.movement.SetDirection(-ghost.movement.direction); // Définit la direction opposée
        }
    }

    // Coroutine pour la transition de sortie du fantôme de la maison
    private IEnumerator ExitTransition()
    {
        // Désactive le mouvement pour animer manuellement la position
        ghost.movement.SetDirection(Vector2.up, true); // Définit la direction du mouvement vers le haut
        ghost.movement.rigidbody.isKinematic = true; // Désactive la physique du rigidbody
        ghost.movement.enabled = false; // Désactive temporairement le mouvement

        Vector3 position = transform.position; // Position actuelle de la maison du fantôme

        float duration = 0.5f; // Durée de la transition en secondes
        float elapsed = 0f; // Temps écoulé pendant la transition

        // Anime le fantôme vers la position intérieure de la maison
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration)); // Interpole la position
            elapsed += Time.deltaTime; // Met à jour le temps écoulé
            yield return null; // Attend la prochaine frame
        }

        elapsed = 0f; // Réinitialise le temps écoulé

        // Anime la sortie du fantôme de la maison
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration)); // Interpole la position
            elapsed += Time.deltaTime; // Met à jour le temps écoulé
            yield return null; // Attend la prochaine frame
        }

        // Choisis une direction aléatoire (gauche ou droite) et réactive le mouvement
        ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true); // Définit la direction du mouvement
        ghost.movement.rigidbody.isKinematic = false; // Réactive la physique du rigidbody
        ghost.movement.enabled = true; // Réactive le mouvement
    }
}
