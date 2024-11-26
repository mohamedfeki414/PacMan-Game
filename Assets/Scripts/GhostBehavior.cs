using UnityEngine;

[RequireComponent(typeof(Ghost))] // Assure que le composant Ghost est attaché à l'objet
public abstract class GhostBehavior : MonoBehaviour
{
    public Ghost ghost { get; private set; } // Référence au Ghost associé à ce comportement
    public float duration; // Durée du comportement

    private void Awake()
    {
        ghost = GetComponent<Ghost>(); // Initialise la référence au Ghost
    }

    public void Enable()
    {
        Enable(duration); // Surcharge la méthode Enable avec la durée spécifiée
    }

    public virtual void Enable(float duration)
    {
        enabled = true; // Active le comportement

        CancelInvoke(); // Annule les invocations précédentes
        Invoke(nameof(Disable), duration); // Désactive le comportement après la durée spécifiée
    }

    public virtual void Disable()
    {
        enabled = false; // Désactive le comportement

        CancelInvoke(); // Annule les invocations précédentes
    }

}
