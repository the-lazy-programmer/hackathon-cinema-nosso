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


    [SerializeField] private Image[] CurrentImage;
    void Start()
    {
        StartCoroutine(TypeSentence());
        //blend imagens
    }

    
    void Update()
    {
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
    /*private void overlapImage(Image currentImage, Image nextImage, float tempoDeSumico) {
        float currenteImageColor = currentImage.GetComponent<Image>().color.a;  
        float nextImageColor = currentImage.GetComponent<Image>().color.a;  
        float contador = 0;
        while (currentImage.color.a > 0)
        {
            contador += Time.deltaTime / tempoDeSumico;
            currenteImageColor = Mathf.Lerp(225 , 0 , contador);

            nextImageColor = Mathf.Lerp(0  , 225 , contador);

            if (currentImage.color.a <= 0)
            {
                Destroy(currentImage);
            }            
        }
        
    }
    /*IEnumerator DesaparecerImage(float tempoDeSumico , Image currentImage) {
        currentImage.gameObject.SetActive(true);
        Color corTexto = currentImage.color;
        corTexto.a = 1;
        currentImage.color = corTexto;
        yield return new WaitForSeconds(2);

        float contador = 0;
        while (currentImage.color.a > 0)
        {
            contador += Time.deltaTime / tempoDeSumico;
            corTexto.a = Mathf.Lerp(1 , 0 , contador);
            currentImage.color = corTexto;
            if (currentImage.color.a <= 0)
            {
                Destroy(gameObject);
            }
            yield return null;
        }*/
    }
