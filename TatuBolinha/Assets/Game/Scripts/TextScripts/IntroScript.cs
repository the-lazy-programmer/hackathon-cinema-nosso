using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour
{
    public TextMeshProUGUI boxText;
    [SerializeField]private Sentence_SO[] currentText;
    private float spechSpeed = .09f;
    private int textIndex = 0;

    public float cronometro = 0;

    void Start()
    {
        StartCoroutine(TypeSentence());
        //blend imagens
    }

    
    void Update()
    {
        cronometro += Time.deltaTime;
        //overlapImage(CurrentImage[0], CurrentImage[1], 1);
        if (textIndex == currentText.Length - 1) return;
        if (boxText.text.Length == currentText[textIndex].Text.ToCharArray().Length - 1)
        {
            Debug.Log("O texto acabou");
            //TUDO Esperar o jogadoer ler o texto
            StopAllCoroutines();
            textIndex++;
            boxText.text = "";
            StartCoroutine(TypeSentence());
        }
    }

    IEnumerator TypeSentence() {
                              //Mudar para um outro bloco de texto
        foreach(char letter in currentText[textIndex].Text.ToCharArray()){
            boxText.text += letter; 
            yield return new WaitForSeconds(spechSpeed);         
        }
    }
    private void nextSentence() {

    }
    
    }
