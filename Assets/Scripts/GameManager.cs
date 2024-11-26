using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Ajout de la directive pour IEnumerator


public class GameManager : MonoBehaviour
{
    public AudioSource siren; // AudioSource pour le son de la sirène
    public AudioSource ghostKillAudio; // Ajout de l'audio pour tuer les fantômes
    public AudioSource pacmanDeathAudio; // Audio de la mort du Pac-Man
    public static GameManager Instance { get; private set; } // Instance statique du GameManager
    
 
    [SerializeField] private Ghost[] ghosts; // Tableau de Ghosts pour gérer les fantômes
    [SerializeField] private Pacman pacman; // Référence au Pacman pour gérer le joueur
    [SerializeField] private Transform pellets; // Transform contenant les pellets du jeu
    [SerializeField] private Text gameOverText; // Texte affichant "Game Over"
    [SerializeField] private Text scoreText; // Texte affichant le score
    [SerializeField] private Text livesText; // Texte affichant le nombre de vies

    private int ghostMultiplier = 1; // Multiplicateur de points des fantômes
    private int lives = 3; // Nombre de vies du joueur
    private int score = 0; // Score du joueur

    public int Lives => lives; // Propriété publique pour accéder au nombre de vies
    public int Score => score; // Propriété publique pour accéder au score

    private void Awake()
    {
        siren.Play(); // Joue le son de la sirène au démarrage
        if (Instance != null) {
            DestroyImmediate(gameObject); // Détruit le GameManager s'il y a déjà une instance existante
        } else {
            Instance = this; // Définit cette instance comme l'instance actuelle du GameManager
            DontDestroyOnLoad(gameObject); // Ne pas détruire cet objet lors du chargement de nouvelles scènes
        }
    }

    private void Start()
    {
        NewGame(); // Démarre une nouvelle partie
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown) {
            NewGame(); // Redémarre une nouvelle partie lorsque le joueur n'a plus de vies et appuie sur une touche
        }
    }

    private void NewGame()
    {
        SetScore(0); // Réinitialise le score à 0
        SetLives(3); // Réinitialise le nombre de vies à 3
        NewRound(); // Démarre un nouveau tour de jeu
    }

    private void NewRound()
    {
        gameOverText.enabled = false; // Désactive le texte "Game Over"

        foreach (Transform pellet in pellets) {
            pellet.gameObject.SetActive(true); // Active tous les pellets du jeu
        }

        ResetState(); // Réinitialise l'état du jeu
    }

    private void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].ResetState(); // Réinitialise l'état de chaque fantôme
        }

        pacman.ResetState(); // Réinitialise l'état du Pacman
    }

    private void GameOver()
    {
        gameOverText.enabled = true; // Active le texte "Game Over"

        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].gameObject.SetActive(false); // Désactive chaque fantôme
        }

        pacman.gameObject.SetActive(false); // Désactive le Pacman
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString(); // Met à jour le texte affichant le nombre de vies
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0'); // Met à jour le texte affichant le score
    }

    public void PacmanEaten()
    {
        pacman.DeathSequence(); // Lance la séquence de mort du Pacman
        pacmanDeathAudio.Play();

        SetLives(lives - 1); // Réduit le nombre de vies du joueur

        if (lives > 0) {
            Invoke(nameof(ResetState), 3f); // Réinitialise l'état du jeu après un délai de 3 secondes
        } else {
            GameOver(); // Déclenche la fin de partie si le joueur n'a plus de vies
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier; // Calcule les points à ajouter au score
        SetScore(score + points); // Met à jour le score avec les points des fantômes

        ghostMultiplier++; // Incrémente le multiplicateur de points des fantômes
        ghostKillAudio.Play(); // Joue l'audio lorsqu'un fantôme est tué
        
    
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false); // Désactive le pellet mangé par le joueur

        SetScore(score + pellet.points); // Ajoute les points du pellet au score

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false); // Désactive le Pacman lorsque tous les pellets sont mangés
            Invoke(nameof(NewRound), 3f); // Démarre un nouveau tour après un délai de 3 secondes
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].frightened.Enable(pellet.duration); // Active l'état effrayé des fantômes pour une durée donnée
        }

        PelletEaten(pellet); // Traite le pellet mangé comme un pellet normal
        CancelInvoke(nameof(ResetGhostMultiplier)); // Annule l'invocation du reset du multiplicateur de points des fantômes
        Invoke(nameof(ResetGhostMultiplier), pellet.duration); // Réinitialise le multiplicateur après la durée du PowerPellet
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf) {
                return true; // Vérifie s'il reste des pellets actifs dans le jeu
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1; // Réinitialise le multiplicateur de points des fantômes à 1
    }
    

}
