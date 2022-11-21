using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Jumping")]
    [SerializeField] AudioClip jumpClip;
    [SerializeField] [Range(0f, 1f)] float jumpVolume = 0.1f;

    [Header("Dashing")]
    [SerializeField] AudioClip dashClip;
    [SerializeField] [Range(0f, 1f)] float dashVolume = 0.1f;

    [Header("Hit")]
    [SerializeField] AudioClip hitClip;
    [SerializeField][Range(0f, 1f)] float hitVolume = 0.1f;

    [Header("Death")]
    [SerializeField] AudioClip dieClip;
    [SerializeField][Range(0f, 1f)] float dieVolume = 0.1f;

    [Header("Coin")]
    [SerializeField] AudioClip coinClip;
    [SerializeField] [Range(0f, 1f)] float coinVolume = 0.1f;

    [Header("1UP")]
    [SerializeField] AudioClip oneUpClip;
    [SerializeField] [Range(0f, 1f)] float oneUpVolume = 0.1f;

    [Header("Exit")]
    [SerializeField] AudioClip exitClip;
    [SerializeField] [Range(0f, 1f)] float exitVolume = 0.1f;

    [Header("Menu Select")]
    [SerializeField] AudioClip clickClip;
    [SerializeField] [Range(0f, 1f)] float clickVolume = 0.1f;

    public void Awake() {
        int numAudioPlayers = FindObjectsOfType<AudioPlayer>().Length;

        if (numAudioPlayers > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void PlayJumpClip() {
        if (jumpClip != null) {
            AudioSource.PlayClipAtPoint(jumpClip,
                                        Camera.main.transform.position,
                                        jumpVolume);
        }
    }
    public void PlayDashClip() {
        if (dashClip != null) {
            AudioSource.PlayClipAtPoint(dashClip,
                                        Camera.main.transform.position,
                                        dashVolume);
        }
    }
    public void PlayHitClip() {
        if (hitClip != null) {
            AudioSource.PlayClipAtPoint(hitClip,
                                        Camera.main.transform.position,
                                        hitVolume);
        }
    }
    public void PlayDieClip() {
        if (dieClip != null) {
            AudioSource.PlayClipAtPoint(dieClip,
                                        Camera.main.transform.position,
                                        dieVolume);
        }
    }
    public void PlayCoinClip() {
        if (coinClip != null) {
            AudioSource.PlayClipAtPoint(coinClip,
                                        Camera.main.transform.position,
                                        coinVolume);
        }
    }
    public void PlayOneUpClip() {
        if (oneUpClip != null) {
            AudioSource.PlayClipAtPoint(oneUpClip,
                                        Camera.main.transform.position,
                                        oneUpVolume);
        }
    }
    public void PlayExitClip() {
        if (exitClip != null) {
            AudioSource.PlayClipAtPoint(exitClip,
                                        Camera.main.transform.position,
                                        exitVolume);
        }
    }
    public void PlayClickClip() {
        if (clickClip != null) {
            AudioSource.PlayClipAtPoint(clickClip,
                                        Camera.main.transform.position,
                                        clickVolume);
        }
    }
}
