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

    //[Header("Pular")]
    //public float PuloAltura = 6.5f;
    //[Range(1f, 1.1f)] public float

    [Header("Debug")]
    public bool DebugMostrarEstaNoChao;
    public bool DebugMostrarCabecaHitbox;
}
