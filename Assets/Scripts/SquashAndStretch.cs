using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquashAndStretch : MonoBehaviour
{
    public float maxStretchAmount = 1.5f;    // Quanto o objeto se estica
    public float maxSquashAmount = 0.5f;     // Quanto o objeto se achata
    public float squashDuration = 0.3f;      // Duração do efeito de squash no final
    public AnimationCurve squashStretchCurve; // Curva para suavizar o ciclo de squash & stretch
    public AnimationCurve squashEndCurve;     // Curva para suavizar o squash no final

    private Vector3 originalScale;           // Escala original do objeto
    private float elapsedTime = 0f;

    public Animator animator;                // Referência ao Animator
    public Button startButton;               // Referência ao botão da UI
    private bool isAnimating = false;        // Controla o início da animação
    private bool isSquashing = false;        // Controla quando aplicar o squash no final
    private float squashStartTime = 0f;      // Marca o momento de início do squash final

    public ParticleSystem _particleSystemLeft;       //Referencia ao sistema de particula do proprio objeto
    public ParticleSystem _particleSystemRight;       //Referencia ao sistema de particula do proprio objeto
    private bool _particleStart = false;

    void Start()
    {
        // Armazena a escala original do objeto
        originalScale = transform.localScale;

        // Verifica se o botão da UI está atribuído e adiciona o Listener
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartSquashStretchAnimation);
        }
    }

    void Update()
    {
        // Verifica se a animação está em andamento
        if (isAnimating)
        {
            // Incrementa o tempo da animação
            elapsedTime += Time.deltaTime;
            float t = (elapsedTime % squashDuration) / squashDuration; // Progresso da animação

            // Verifica se está no final da animação para aplicar o squash
            if (isSquashing)
            {
                // Aplica o squash no final da animação
                float squashProgress = (Time.time - squashStartTime) / squashDuration;
                float squashValue = Mathf.Lerp(1f, maxSquashAmount, squashEndCurve.Evaluate(squashProgress));
                
                // Aplica o squash apenas no eixo Y
                transform.localScale = new Vector3(originalScale.x, originalScale.y * squashValue, originalScale.z);

                // Para o squash após a duração
                if (squashProgress >= 1f)
                {
                    isSquashing = false;
                    isAnimating = false; // Finaliza a animação
                    ResetToOriginalScale(); // Reseta a escala após o squash
                }
            }
            else
            {
                // Aplica o squash & stretch normal durante a animação
                float curveValue = squashStretchCurve.Evaluate(t);
                float stretch = Mathf.Lerp(1f, maxStretchAmount, curveValue);
                float squash = Mathf.Lerp(1f, maxSquashAmount, curveValue);

                // Ajusta a escala para o squash & stretch
                transform.localScale = new Vector3(originalScale.x * squash, originalScale.y * stretch, originalScale.z * squash);
            }

            // Detecta o final da animação e inicia o squash
            if (t >= 0.9f && !isSquashing)
            {
                StartSquashFinal();
            }
        }
    }

    // Método para começar o efeito de squash no final
    private void StartSquashFinal()
    {
        isSquashing = true;
        squashStartTime = Time.time;
        _particleSystemLeft.Play();
        _particleSystemRight.Play();
    }

    // Método para redefinir a escala original do objeto
    private void ResetToOriginalScale()
    {
        transform.localScale = originalScale;
    }

    // Função para iniciar a animação ao clicar no botão
    public void StartSquashStretchAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("StartSquashStretch");
        }

        // Ativa o Squash & Stretch manualmente
        isAnimating = true;
        elapsedTime = 0f;
    }
}