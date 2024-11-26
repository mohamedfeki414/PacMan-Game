using UnityEngine;

public class GhostScatter : GhostBehavior // Hérite de GhostBehavior pour définir le comportement de dispersion du fantôme
{
    private void OnDisable()
    {
        ghost.chase.Enable(); // Active le comportement de poursuite lorsque le comportement de dispersion est désactivé
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>(); // Récupère le composant Node du collider

        // Ne rien faire lorsque le fantôme est effrayé
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            // Choisis une direction disponible aléatoire
            int index = Random.Range(0, node.availableDirections.Count);

            // Préfère ne pas retourner dans la même direction, donc incrémente l'index
            // vers la prochaine direction disponible
            if (node.availableDirections.Count > 1 && node.availableDirections[index] == -ghost.movement.direction)
            {
                index++;

                // Remet l'index à zéro s'il dépasse le nombre de directions disponibles
                if (index >= node.availableDirections.Count) {
                    index = 0;
                }
            }

            ghost.movement.SetDirection(node.availableDirections[index]); // Définit la direction de déplacement du fantôme
        }
    }

}

