using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))] // Assure que le composant SpriteRenderer est attaché à l'objet
public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[0]; // Tableau de sprites pour l'animation
    public float animationTime = 0.25f; // Temps entre chaque changement de sprite
    public bool loop = true; // Indique si l'animation doit boucler

    private SpriteRenderer spriteRenderer; // Référence au composant SpriteRenderer
    private int animationFrame; // Indice du sprite actuel dans le tableau

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Initialise la référence au composant SpriteRenderer
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true; // Assure que le sprite est activé lorsque l'objet est activé
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false; // Désactive le sprite lorsque l'objet est désactivé
    }

    private void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime); // Lance la méthode Advance de manière répétée à intervalles réguliers
    }

    private void Advance()
    {
        if (!spriteRenderer.enabled) {
            return; // Arrête l'avancement de l'animation si le sprite est désactivé
        }

        animationFrame++; // Passe au sprite suivant

        if (animationFrame >= sprites.Length && loop) {
            animationFrame = 0; // Réinitialise l'animation si elle boucle et atteint la fin du tableau
        }

        if (animationFrame >= 0 && animationFrame < sprites.Length) {
            spriteRenderer.sprite = sprites[animationFrame]; // Change le sprite affiché selon l'indice actuel
        }
    }

    public void Restart()
    {
        animationFrame = -1; // Réinitialise l'indice de l'animation à -1 pour redémarrer depuis le début
        Advance(); // Lance l'avancement de l'animation
    }
}
