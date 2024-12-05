using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Para usar TextMeshPro

public class AnimatorAudioController : MonoBehaviour
{
    public Animator animator; // Referência ao Animator
    private AudioSource audioSource;

    public AudioClip breakdanceMusic;
    public AudioClip flairMusic;
    public AudioClip hipHopMusic;

    public TextMeshProUGUI danceNameText; // Referência ao texto da UI

    private string[] danceNames = { "Breakdance", "Flair", "Hip Hop" };
    private AudioClip[] danceClips;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        danceClips = new AudioClip[] { breakdanceMusic, flairMusic, hipHopMusic };
        UpdateDanceName(""); // Inicializa o texto vazio
    }

    void Update()
    {
        // Detecta entradas do usuário para mudar o estilo de dança
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetDanceStyle(0); // Breakdance
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetDanceStyle(0.5f); // Flair
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetDanceStyle(1); // Hip Hop
        }
    }

    void SetDanceStyle(float targetStyle)
    {
        StartCoroutine(SmoothTransition(targetStyle)); // Faz a transição suave do estilo de dança
        int styleIndex = Mathf.RoundToInt(targetStyle * 2); // Converte o valor para índice do array
        PlayMusic(danceClips[styleIndex]); // Toca a música correspondente
        UpdateDanceName(danceNames[styleIndex]); // Atualiza o texto
    }

    IEnumerator SmoothTransition(float targetStyle)
    {
        float currentStyle = animator.GetFloat("DanceStyle"); // Obtém o valor atual do parâmetro
        float transitionSpeed = 2f; // Velocidade da transição (ajuste conforme necessário)

        // Faz o valor transitar suavemente até o alvo
        while (Mathf.Abs(currentStyle - targetStyle) > 0.01f)
        {
            currentStyle = Mathf.Lerp(currentStyle, targetStyle, Time.deltaTime * transitionSpeed);
            animator.SetFloat("DanceStyle", currentStyle);
            yield return null;
        }

        // Garante que o parâmetro final seja exatamente o alvo
        animator.SetFloat("DanceStyle", targetStyle);
    }

    void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return; // Evita reiniciar a música se já está tocando
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    void UpdateDanceName(string danceName)
    {
        if (danceNameText != null)
        {
            danceNameText.text = danceName; // Atualiza o texto na UI
        }
    }
}
