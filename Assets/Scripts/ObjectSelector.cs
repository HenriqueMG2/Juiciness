using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public GameObject[] objects;  // Array com os objetos
    public GameObject[] startButtons;  // Array com os bot�es "Start" correspondentes
    private Vector3[] initialPositions;  // Array para armazenar as posi��es iniciais dos objetos
    public GameObject leftArrow;  // Refer�ncia para o bot�o da seta esquerda
    public GameObject rightArrow; // Refer�ncia para o bot�o da seta direita
    private int currentIndex = 0; // �ndice do objeto selecionado

    void Start()
    {
        // Inicializa o array de posi��es iniciais
        initialPositions = new Vector3[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            initialPositions[i] = objects[i].transform.position;  // Salva a posi��o inicial de cada objeto
        }

        // Atualiza a sele��o inicial (mostra o primeiro objeto)
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

    // Fun��o p�blica para navegar para o pr�ximo objeto
    public void NextObject()
    {
        currentIndex = (currentIndex + 1) % objects.Length;  // Avan�a para o pr�ximo objeto
        UpdateSelection();
    }

    // Fun��o p�blica para voltar para o objeto anterior
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

    // Atualiza a sele��o do objeto e bot�o "Start"
    void UpdateSelection()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            // Ativa o objeto e o bot�o "Start" apenas para o objeto selecionado
            bool isSelected = (i == currentIndex);
            objects[i].SetActive(isSelected);
            startButtons[i].SetActive(isSelected);

            // Se n�o for o objeto selecionado, resetar a posi��o dele
            if (!isSelected)
            {
                ResetPosition(objects[i], initialPositions[i]);
            }
        }

        // As setas sempre devem permanecer vis�veis para navega��o
        leftArrow.SetActive(true);
        rightArrow.SetActive(true);
    }

    // Fun��o para resetar a posi��o do objeto para a posi��o inicial
    void ResetPosition(GameObject obj, Vector3 initialPosition)
    {
        obj.transform.position = initialPosition;  // Reseta a posi��o do objeto para a posi��o inicial
    }
}
