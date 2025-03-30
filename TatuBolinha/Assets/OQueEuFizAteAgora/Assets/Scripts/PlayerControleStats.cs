using UnityEngine;
using UnityEngine.WSA;

[CreateAssetMenu(menuName = "Movimento do Jogador")]
public class PlayerControleStats : ScriptableObject
{
    [Header("Andar")]
    [Range(1f, 100f)] public float VelMaxAndar = 12.5f;
    [Range(0.25f, 50f)] public float AceleracaoChao = 5f;
    [Range(0.25f, 50f)] public float DesaceleracaoChao = 20f;
    [Range(0.25f, 50f)] public float AceleracaoNoAr = 5f;
    [Range(0.25f, 50f)] public float DesaceleracaoNoAr = 5f;

    [Header("Correr")]
    [Range(1f, 100f)] public float VelMaxCorrer = 20f;

    [Header("Cheque de Colisão")]
    public LayerMask LayerChao;
    public float DeteccaoChaoComprimento = 0.02f;
    public float DeteccaoCabecaComprimento = 0.02f;
    [Range(0f, 1f)] public float LarguraCabeca = 0.75f;

    [Header("Pular")]
    public float PuloAltura = 6.5f;
    [Range(1f, 1.1f)] public float PuloAlturaFatorDeCompensacao = 1.054f;
    public float TempoAtePicoDoPulo = 0.35f;
    [Range(0.01f, 5f)] public float GravidadeAoCairMultiplicador = 2f;
    public float VelMaxAoCair = 26f;
    [Range(1, 5)] public int NumDePulosPermitidos = 2;

    [Header("Cancelamento do Pulo")]
    [Range(0.02f, 0.3f)] public float TempoParaCancelamento = 0.027f;

    [Header("Pico do Pulo")]
    [Range(0.5f, 1f)] public float ComecoDoPico = 0.97f;
    [Range(0.01f, 1f)] public float TempoParadoNoPico = 0.075f;

    [Header("Buffer do Pulo")]
    [Range(0f, 1f)] public float TempoDeBuffer = 0.125f;

    [Header("''Coyote Time''")]
    [Range(0f, 1f)] public float CoyoteTime = 0.1f;

    [Header("Debug")]
    public bool DebugMostrarEstaNoChao;
    public bool DebugMostrarCabecaHitbox;

    [Header("Ferramenta de Visualisação de Pulo")]
    public bool MostrarArcoDoPulo = false;
    public bool MostrarArcoDePuloComCorrer = false;
    public bool PararComAColisao = true;
    public bool DrawPelaDireita = true;
    [Range(5, 100)] public int ResolucaoDoArco = 20;
    [Range(0, 500)] public int PassosDeVisualisacao = 90;
    
    public float Gravidade {get; private set; }
    public float VelocidadeInicialDoPulo {get; private set; }
    public float AlturaAjustadaDoPulo {get; private set; }

    private void OnValidate()
    {
        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();
    }

    private void CalculateValues()
    {
        AlturaAjustadaDoPulo = PuloAltura * PuloAlturaFatorDeCompensacao;
        Gravidade = -(2f * AlturaAjustadaDoPulo) / Mathf.Pow(TempoAtePicoDoPulo, 2f);
        VelocidadeInicialDoPulo = Mathf.Abs(Gravidade) * TempoAtePicoDoPulo;
    }

}
