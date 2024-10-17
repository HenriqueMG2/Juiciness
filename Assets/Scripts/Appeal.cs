using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Appeal : MonoBehaviour
{
   public float maxScaleChange = 1.2f;          // Intensidade máxima de mudança de escala (esticar ou achatar)
    public float duration = 1f;                  // Duração total da animação
    public AnimationCurve appealCurve;           // Curva de suavização para o apelo visual

    private Vector3 originalScale;               // Guarda a escala original do objeto
    private float elapsedTime = 0f;
    private bool isAnimating = false;

    public Button startButton;                   // Referência ao botão da UI

    public ParticleSystem _particleSystem;       //Referencia ao sistema de particula do proprio objeto        

    void Start()
    {
        // Armazena a escala original do objeto
        originalScale = transform.localScale;

        // Se o botão de start for configurado, adiciona o listener
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartAppealAnimation);
        }
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (isAnimating)
        {
            // Incrementa o tempo da animação
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Avalia a curva de apelo
            float curveValue = appealCurve.Evaluate(t);

            // Aplica a deformação de apelo
            float scaleValue = Mathf.Lerp(1f, maxScaleChange, curveValue);

            // Ajusta a escala para ser simétrica e manter apelo visual
            transform.localScale = new Vector3(originalScale.x * scaleValue, originalScale.y * scaleValue, originalScale.z);

            // Quando o tempo termina, reseta
            if (elapsedTime >= duration)
            {
                isAnimating = false;
                ResetToOriginalScale();
            }
        }
    }

    // Inicia a animação ao clicar no botão
    public void StartAppealAnimation()
    {
        isAnimating = true;
        elapsedTime = 0f;
        _particleSystem.Play();
    }

    // Método para resetar a escala original
    private void ResetToOriginalScale()
    {
        transform.localScale = originalScale;
    }
}
