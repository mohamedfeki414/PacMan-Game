using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public LayerMask obstacleLayer; // Masque de collision pour les obstacles
    public readonly List<Vector2> availableDirections = new(); // Liste des directions disponibles

    private void Start()
    {
        availableDirections.Clear(); // Efface la liste des directions disponibles

        // On détermine si la direction est disponible en effectuant une boxcast pour voir
        // si nous heurtons un mur. La direction est ajoutée à la liste si elle est disponible.
        CheckAvailableDirection(Vector2.up); // Vérifie la direction vers le haut
        CheckAvailableDirection(Vector2.down); // Vérifie la direction vers le bas
        CheckAvailableDirection(Vector2.left); // Vérifie la direction vers la gauche
        CheckAvailableDirection(Vector2.right); // Vérifie la direction vers la droite
    }

    private void CheckAvailableDirection(Vector2 direction)
    {
        // Effectue un boxcast pour vérifier s'il y a un obstacle dans la direction spécifiée
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, obstacleLayer);

        // S'il n'y a pas de collider touché, cela signifie qu'il n'y a pas d'obstacle dans cette direction
        if (hit.collider == null) {
            availableDirections.Add(direction); // Ajoute la direction à la liste des directions disponibles
        }
    }

}

