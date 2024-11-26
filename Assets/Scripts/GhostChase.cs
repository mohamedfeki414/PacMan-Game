using UnityEngine;

public class GhostChase : GhostBehavior // Hérite de la classe GhostBehavior pour définir un comportement de poursuite
{
    private void OnDisable()
    {
        ghost.scatter.Enable(); // Active le comportement de dispersion lorsque le comportement de poursuite est désactivé
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>(); // Récupère le composant Node du collider

        // Ne rien faire lorsque le fantôme est effrayé
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero; // Direction initiale
            float minDistance = float.MaxValue; // Distance minimale initiale

            // Trouve la direction disponible qui se rapproche le plus du Pacman
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // Si la distance dans cette direction est inférieure à la distance minimale actuelle
                // alors cette direction devient la nouvelle direction la plus proche
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude; // Calcule la distance au carré pour des raisons de performance

                if (distance < minDistance)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction); // Définit la direction de déplacement du fantôme
        }
    }

}
