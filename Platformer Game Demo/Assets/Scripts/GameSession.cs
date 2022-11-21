using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 2;
    [SerializeField] public int playerHealth = 3;
    [SerializeField] int playerHearts = 3;
    [SerializeField] public int coins = 95;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI coinsText;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private AudioPlayer audioPlayer;

    void Awake() {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        
        if (numGameSessions > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() {
        livesText.text = "<sprite=33> <sprite=" + playerLives + ">";
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void Update() {
        for (int i = 0; i < hearts.Length; i++) {
            if (i < playerHealth) {
                hearts[i].sprite = fullHeart;
            }
            else {
                hearts[i].sprite = emptyHeart;
            }

            if (i < playerHearts) {
                hearts[i].enabled = true;
            }
            else {
                hearts[i].enabled = false;
            }
        }

        coinsText.text = "<sprite=33> <sprite=" + (coins / 10) +"><sprite=" + (coins % 10) +">";
    }

    public void ProcessPlayerDamage() {
        playerHealth--;
    }

    public void ProcessPlayerDeath() {
        if (playerLives > 0) {
            TakeLife();
        } else {
            ResetGameSession();
        }
    }

    void TakeLife() {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        playerHealth = playerHearts;
        livesText.text = "<sprite=33> <sprite=" + playerLives + ">";
    }

    public void ResetGameSession() {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void CoinPickup(int coinValue) {
        audioPlayer.PlayCoinClip();
        coins+= coinValue;
        if (coins >= 100) {
            audioPlayer.PlayOneUpClip();
            coins -= 100;
            if (playerLives < 9) {
                playerLives++;
                livesText.text = "<sprite=33> <sprite=" + playerLives + ">";
            }
        }
    }
}
