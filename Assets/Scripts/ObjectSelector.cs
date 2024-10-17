using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public GameObject[] objects;  // Array com os objetos
    public GameObject[] startButtons;  // Array com os botões "Start" correspondentes
    private Vector3[] initialPositions;  // Array para armazenar as posições iniciais dos objetos
    public GameObject leftArrow;  // Referência para o botão da seta esquerda
    public GameObject rightArrow; // Referência para o botão da seta direita
    private int currentIndex = 0; // Índice do objeto selecionado

    void Start()
    {
        // Inicializa o array de posições iniciais
        initialPositions = new Vector3[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            initialPositions[i] = objects[i].transform.position;  // Salva a posição inicial de cada objeto
        }

        // Atualiza a seleção inicial (mostra o primeiro objeto)
        UpdateSelection();
    }

    void Update()
    {
        // Detecta as teclas de seta para navegar entre os objetos
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextObject();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousObject();
        }
    }

    // Função pública para navegar para o próximo objeto
    public void NextObject()
    {
        currentIndex = (currentIndex + 1) % objects.Length;  // Avança para o próximo objeto
        UpdateSelection();
    }

    // Função pública para voltar para o objeto anterior
    public void PreviousObject()
    {
        currentIndex = (currentIndex - 1 + objects.Length) % objects.Length;  // Volta para o objeto anterior
        UpdateSelection();
    }
    public void QuitGame()
    {
        Debug.Log("Sair do Jogo.");
        Application.Quit();
    }

    // Atualiza a seleção do objeto e botão "Start"
    void UpdateSelection()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            // Ativa o objeto e o botão "Start" apenas para o objeto selecionado
            bool isSelected = (i == currentIndex);
            objects[i].SetActive(isSelected);
            startButtons[i].SetActive(isSelected);

            // Se não for o objeto selecionado, resetar a posição dele
            if (!isSelected)
            {
                ResetPosition(objects[i], initialPositions[i]);
            }
        }

        // As setas sempre devem permanecer visíveis para navegação
        leftArrow.SetActive(true);
        rightArrow.SetActive(true);
    }

    // Função para resetar a posição do objeto para a posição inicial
    void ResetPosition(GameObject obj, Vector3 initialPosition)
    {
        obj.transform.position = initialPosition;  // Reseta a posição do objeto para a posição inicial
    }
}
