using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowThroughOverlapping : MonoBehaviour
{
    public float maxBendAmount = 0.5f;        // Intensidade máxima de curvatura
    public float duration = 2f;               // Duração da deformação
    public float delay = 0.5f;                // Atraso para seguir o movimento principal
    public float returnDuration = 2f;         // Duração do retorno à forma original
    public AnimationCurve easingCurve;        // Curva de easing para suavidade
    public Animator animator;                // Referência ao Animator
    public Button startButton;                // Referência ao botão da UI

    private Mesh originalMesh;
    private Mesh deformedMesh;
    private float elapsedTime = 0f;
    private bool isDeforming = false;
    private bool isReturning = false;         // Para controlar a fase de retorno

    public ParticleSystem _particleSystem;       //Referencia ao sistema de particula do proprio objeto        

    void Start()
    {
        // Inicializa o mesh original e a deformação
        originalMesh = GetComponent<MeshFilter>().mesh;
        deformedMesh = Instantiate(originalMesh);
        GetComponent<MeshFilter>().mesh = deformedMesh;

        // Obtém o Animator do objeto
        animator = GetComponent<Animator>();

        // Conecta o botão da UI à função StartDeformation
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartDeformation);
        }

        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (isDeforming)
        {
            // Calcula o tempo para a animação
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01((elapsedTime - delay) / duration); // Aplica o atraso
            float easedT = easingCurve.Evaluate(t); // Aplica a curva de easing

            // Calcula a deformação com o atraso
            if (t > 0) // Só começa a deformar após o delay
            {
                float bendAmount = easedT * maxBendAmount;
                BendMesh(bendAmount);
            }

            // Verifica se o tempo da deformação acabou
            if (elapsedTime >= duration + delay)
            {
                isDeforming = false;
                isReturning = true; // Inicia a fase de retorno
                elapsedTime = 0f;   // Reseta o tempo para o retorno
                _particleSystem.Stop();
                //_particleSystem.Clear();
            }
        }

        if (isReturning)
        {
            // Calcula o tempo para a animação de retorno
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / returnDuration); // Progresso do retorno
            float easedT = easingCurve.Evaluate(1f - t); // Aplica o easing inverso

            // Calcula a deformação reversa para retornar à forma original
            float bendAmount = easedT * maxBendAmount;
            BendMesh(bendAmount);

            // Verifica se o tempo de retorno acabou
            if (elapsedTime >= returnDuration)
            {
                isReturning = false; // Termina o retorno
            }
        }
    }

    void BendMesh(float bendAmount)
    {
        Vector3[] vertices = originalMesh.vertices;
        Vector3[] deformedVertices = new Vector3[vertices.Length];

        float bendThreshold = 0.0f; // Define a altura a partir da qual o bend será aplicado (valor mínimo no eixo Y)

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            
            // Só aplica a deformação para os vértices acima do threshold (parte superior do objeto)
            if (vertex.y > bendThreshold)
            {
                float curveFactor = Mathf.Sin((vertex.y - bendThreshold) * bendAmount) * Mathf.Exp(-(vertex.y - bendThreshold) * bendAmount);
                vertex.x += curveFactor; // Aplica a curvatura no eixo X para a parte superior
            }

            deformedVertices[i] = vertex;
        }

        deformedMesh.vertices = deformedVertices;
        deformedMesh.RecalculateNormals();
    }

    // Método público para iniciar a deformação
    public void StartDeformation()
    {
        // Reinicia a animação no Animator
        if (animator != null)
        {
            animator.SetTrigger("StartMove");
            _particleSystem.Play();
        }

        elapsedTime = 0f;
        isDeforming = true;
        isReturning = false; // Garante que o retorno ainda não começou
    }
}