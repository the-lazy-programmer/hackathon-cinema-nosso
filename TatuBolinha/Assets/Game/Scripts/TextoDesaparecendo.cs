
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextoDesaparecendo : MonoBehaviour
{
    private TMP_Text TextoBossAparece;
    private void Start() {
        TextoBossAparece = GetComponent<TMP_Text>();
        AparecerTextoCriado();
    }
    public void AparecerTextoCriado() {
        StartCoroutine(DesaparecerTexto(2 , TextoBossAparece));
    }
    IEnumerator DesaparecerTexto(float tempoDeSumico , TMP_Text textoParaSumir) {
        textoParaSumir.gameObject.SetActive(true);
        Color corTexto = textoParaSumir.color;
        corTexto.a = 1;
        textoParaSumir.color = corTexto;
        yield return new WaitForSeconds(2);

        float contador = 0;
        while (textoParaSumir.color.a > 0)
        {
            contador += Time.deltaTime / tempoDeSumico;
            corTexto.a = Mathf.Lerp(1 , 0 , contador);
            textoParaSumir.color = corTexto;
            if (textoParaSumir.color.a < 0)
            {
                textoParaSumir.gameObject.SetActive(false);
            }
            yield return null;
        }
    }
}
