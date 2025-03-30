using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq.Expressions;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine.AI;

public class PlayerMovimento : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerControleStats MoveStats;
    [SerializeField] private Collider2D peCollisao;
    [SerializeField] private Collider2D corpoCollisao;
   
    Rigidbody2D rb;

    // Variáveis de movimento:
    private Vector2 velMovimento;
    private bool olhandoParaDireita;

    // Variáveis de Colisão:
    private RaycastHit2D chaoAtingido;
    private RaycastHit2D cabecaHitbox;
    private bool estaNoChao;
    private bool bateuCabeca;

    // Variáveis de Pulo:
    public float VelocidadeVertical { get; private set; }
    private bool estaPulando;
    private bool estaCaindoRapido;
    private bool estaCaindo;
    private float tempoDaQuedaRapida;
    private float velDaQuedaRapidaQuandoTeclaESoltada;
    private int numDePulosUsados;

    // Variáveis do Pico do Pulo:
    private float pontoDePico;
    private float tempoDepoisDoComecoDoPico;
    private bool passouDoComecoDoPico;

    // Variáveis do Buffer do Pulo:
    private float tempoDeBufferDoPulo;
    private bool teclaSoltadaDuranteBuffer;

    // Variáveis do "Coyote Time":
    private float coyoteTimer;  

    void Awake()
    {
        olhandoParaDireita = true;

        rb = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        ConteOsTimers();
        ChecarPulo();
    }

    private void FixedUpdate()
    {
        ChequeColisao();
        Pulo();

        if(estaNoChao)
        {
            Move(MoveStats.AceleracaoChao, MoveStats.DesaceleracaoChao, InputManager.Movement);
        }
        else
        {
            Move(MoveStats.AceleracaoNoAr, MoveStats.DesaceleracaoNoAr, InputManager.Movement);
        }
    }

    #region Movimento

    private void Move(float aceleracao, float desaceleracao, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);

            Vector2 targetVelocity = Vector2.zero;
            if (InputManager.runHeld)
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.VelMaxCorrer;
            }
            else
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.VelMaxAndar;
            }

            velMovimento = Vector2.Lerp(velMovimento, targetVelocity, aceleracao * Time.fixedDeltaTime);
            rb.velocity = new Vector2(velMovimento.x, rb.velocity.y);
        }

        else if (moveInput == Vector2.zero)
        {
            velMovimento = Vector2.Lerp(velMovimento, Vector2.zero, desaceleracao * Time.fixedDeltaTime);
            rb.velocity = new Vector2(velMovimento.x, velMovimento.y);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (olhandoParaDireita && moveInput.x < 0)
        {
            Turn(false);
        }

        else if (!olhandoParaDireita && moveInput.x > 0)
        {
            Turn(true);
        }
    }

    private void Turn(bool virarParaDireita)
    {
        if(virarParaDireita)
        {
            olhandoParaDireita = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            olhandoParaDireita = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    #endregion
    
    #region Pulo

    private void ChecarPulo()
    {
        // Quando o botão de pular é apertado
        if(InputManager.jumpPressed)
        {
            tempoDeBufferDoPulo = MoveStats.TempoDeBuffer;
            teclaSoltadaDuranteBuffer = false;
        }
        
        // Quando o botão é soltado
        if(InputManager.jumpReleased)
        {
            if(tempoDeBufferDoPulo > 0f)
            {
                teclaSoltadaDuranteBuffer = true;
            }

            if(estaPulando && VelocidadeVertical > 0f)
            {
                if(passouDoComecoDoPico)
                {
                    passouDoComecoDoPico = false;
                    estaCaindoRapido = true;
                    tempoDaQuedaRapida = MoveStats.TempoParaCancelamento;
                    VelocidadeVertical = 0f;
                }
                else
                {
                    estaCaindoRapido = true;
                    velDaQuedaRapidaQuandoTeclaESoltada = VelocidadeVertical;
                }
            }
        }

        // Iniciando pulo com buffer e coyote time
        if(tempoDeBufferDoPulo > 0f && !estaPulando && (estaNoChao || coyoteTimer > 0f))
        {
            ComecarPulo(1);

            if(teclaSoltadaDuranteBuffer)
            {
                estaCaindoRapido = true;
                velDaQuedaRapidaQuandoTeclaESoltada = VelocidadeVertical;   
            }
        }

        // Quaisquer pulos no ar depois do primeiro
        else if(tempoDeBufferDoPulo > 0f && estaPulando && numDePulosUsados < MoveStats.NumDePulosPermitidos)
        {
            estaCaindoRapido = false;
            ComecarPulo(1);
        }

        // Pulo no ar depois que o coyote time terminou (tirando um pulo para não ter um pulo adicional)
        else if(tempoDeBufferDoPulo > 0f && estaPulando && numDePulosUsados < MoveStats.NumDePulosPermitidos - 1)
        {
            ComecarPulo(2);
            estaCaindoRapido = false;
        }

        // Caiu no chão
        if((estaPulando || estaCaindo) && estaNoChao && VelocidadeVertical <= 0f)
        {
            estaPulando = false;
            estaCaindo = false;
            estaCaindoRapido = false;
            tempoDaQuedaRapida = 0f;
            passouDoComecoDoPico = false;
            numDePulosUsados = 0;

            VelocidadeVertical = Physics2D.gravity.y;
        }

    }

    private void ComecarPulo(int NumDePulosUsados)
    {
        if (!estaPulando)
        {
            estaPulando = true;
        }

        tempoDeBufferDoPulo = 0f;
        numDePulosUsados += NumDePulosUsados;
        VelocidadeVertical = MoveStats.VelocidadeInicialDoPulo;
    }

    private void Pulo()
    {
        // Aplicar Gravidade No Pulo
        if(estaPulando)
        {
            // Checar Se A Cabeca Bateu Em Alguma Coisa
            if(bateuCabeca)
            {
                estaCaindoRapido = true;
            }

            // Gravidade Enquanto Sobe
            if(VelocidadeVertical >= 0f)
            {
                // Controles do Pico do Pulo
                pontoDePico = Mathf.InverseLerp(MoveStats.VelocidadeInicialDoPulo, 0f, VelocidadeVertical);

                if(pontoDePico > MoveStats.ComecoDoPico)
                {
                    if(!passouDoComecoDoPico)
                    {
                        passouDoComecoDoPico = true;
                        tempoDepoisDoComecoDoPico = 0f;
                    }

                    if(passouDoComecoDoPico)
                    {
                        tempoDepoisDoComecoDoPico += Time.fixedDeltaTime;
                        if(tempoDepoisDoComecoDoPico < MoveStats.TempoParadoNoPico)
                        {
                            VelocidadeVertical = 0f;
                        }
                        else
                        {
                            VelocidadeVertical = -0.01f;
                        }
                    }
                }

                // Gravidade Enquanto Sobe Mas Antes Do Comeco Do Pico
                else
                {
                    VelocidadeVertical += MoveStats.Gravidade * Time.fixedDeltaTime;
                    if(passouDoComecoDoPico)
                    {
                        passouDoComecoDoPico = false;
                    }
                }

            }

            // Gravidade Enquanto Desce
            else if(!estaCaindoRapido)
            {
                VelocidadeVertical += MoveStats.Gravidade * MoveStats.GravidadeAoCairMultiplicador * Time.fixedDeltaTime;
            }

            else if(VelocidadeVertical < 0f)
            {
                if(!estaCaindo)
                {
                    estaCaindo = true;
                }
            }
        }

        // Pulo Cortado
        if(estaCaindoRapido)
        {
            if(tempoDaQuedaRapida >= MoveStats.TempoParaCancelamento)
            {
                VelocidadeVertical += MoveStats.Gravidade * MoveStats.GravidadeAoCairMultiplicador * Time.fixedDeltaTime;
            }
            else if(tempoDaQuedaRapida < MoveStats.TempoParaCancelamento)
            {
                VelocidadeVertical = Mathf.Lerp(velDaQuedaRapidaQuandoTeclaESoltada, 0f, (tempoDaQuedaRapida / MoveStats.TempoParaCancelamento));
            }

            tempoDaQuedaRapida += Time.fixedDeltaTime;
        }

        // Gravidade Normal Enquanto Cai
        if(!estaNoChao && !estaPulando)
        {
            if(!estaCaindo)
            {
                estaCaindo = true;
            }

            VelocidadeVertical += MoveStats.Gravidade * Time.fixedDeltaTime;
        }

        // Limitando A Velocidade de Queda
        VelocidadeVertical = Mathf.Clamp(VelocidadeVertical, -MoveStats.VelMaxAoCair, 50f);

        rb.velocity = new Vector2(rb.velocity.x, VelocidadeVertical);
        rb.velocity = new Vector2(rb.velocity.x , VelocidadeVertical);

    }

    private void DrawArcoDoPulo(float velAndar, Color gizmoCor)
    {
        Vector2 posicaoInicial = new Vector2(peCollisao.bounds.center.x, peCollisao.bounds.min.y);
        Vector2 posicaoAnterior = posicaoInicial;
        float velocidade = 0f;
        if(MoveStats.DrawPelaDireita)
        {
            velocidade = velAndar;
        }
        else
        {
            velocidade = -velAndar;
        }
        Vector2 velocidadePlayer = new Vector2(velocidade, MoveStats.VelocidadeInicialDoPulo);

        Gizmos.color = gizmoCor;

        float passoTempo = 2 * MoveStats.TempoAtePicoDoPulo / MoveStats.ResolucaoDoArco;

        for(int i = 0; i < MoveStats.PassosDeVisualisacao; i++)
        {
            float tempoDaSimulacao = i * passoTempo;
            Vector2 deslocamento;
            Vector2 pontoDeDraw;

            if(tempoDaSimulacao < MoveStats.TempoAtePicoDoPulo)
            {
                deslocamento = velocidadePlayer * tempoDaSimulacao + 0.5f * new Vector2(0, MoveStats.Gravidade) * tempoDaSimulacao * tempoDaSimulacao;
            }else if(tempoDaSimulacao < MoveStats.TempoAtePicoDoPulo + MoveStats.TempoParadoNoPico) // Subindo
            {
                float tempoNoPico = tempoDaSimulacao - MoveStats.TempoParadoNoPico;
                deslocamento = velocidadePlayer * MoveStats.TempoAtePicoDoPulo + 0.5f * new Vector2(0, MoveStats.Gravidade) * MoveStats.TempoAtePicoDoPulo * MoveStats.TempoAtePicoDoPulo;
                deslocamento += new Vector2(velocidade, 0) * tempoNoPico; // Nenhum movimento vertical durante o pico
            }
            else // Descendo
            {
                float tempoDescendo = tempoDaSimulacao - (MoveStats.TempoAtePicoDoPulo + MoveStats.TempoParadoNoPico);
                deslocamento = velocidadePlayer * MoveStats.TempoAtePicoDoPulo + 0.5f * new Vector2(0, MoveStats.Gravidade) * MoveStats.TempoAtePicoDoPulo * MoveStats.TempoAtePicoDoPulo;
                deslocamento += new Vector2(velocidade, 0) * MoveStats.TempoParadoNoPico;
                deslocamento += new Vector2(velocidade, 0) * tempoDescendo + 0.5f * new Vector2(0, MoveStats.Gravidade) * tempoDescendo * tempoDescendo;
            }

            pontoDeDraw = posicaoInicial + deslocamento;

            if(MoveStats.PararComAColisao)
            {
                RaycastHit2D hit = Physics2D.Raycast(posicaoAnterior, pontoDeDraw - posicaoAnterior, Vector2.Distance(posicaoAnterior, pontoDeDraw), MoveStats.LayerChao);
                if(hit.collider != null)
                {
                    // Se a linha colidir com algo, para de desenhar
                    Gizmos.DrawLine(posicaoAnterior, hit.point);
                    break;
                }
            }

            Gizmos.DrawLine(posicaoAnterior, pontoDeDraw);
            posicaoAnterior = pontoDeDraw;

        }
    }

    private void OnDrawGizmos()
    {
        if (MoveStats.MostrarArcoDoPulo)
        {
            DrawArcoDoPulo(MoveStats.VelMaxAndar, Color.white);
        }

        if (MoveStats.MostrarArcoDePuloComCorrer)
        {
            DrawArcoDoPulo(MoveStats.VelMaxCorrer, Color.red);
        }
    }

    #endregion

    #region Cheque de Colisão

    private void noChao()
    {
        Vector2 boxCastOrigin = new Vector2(peCollisao.bounds.center.x, peCollisao.bounds.min.y);
        Vector2 boxCastSize = new Vector2(peCollisao.bounds.size.x, MoveStats.DeteccaoChaoComprimento);

        chaoAtingido = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.DeteccaoChaoComprimento, MoveStats.LayerChao);
        if(chaoAtingido.collider != null)
        {
            estaNoChao = true;
        }
        else
        {
            estaNoChao = false;
        }

        #region Visualição Debug
        if(MoveStats.DebugMostrarEstaNoChao)
        {
            Color corRaio;
            if(estaNoChao)
            {
                corRaio = Color.green;
            }
            else 
            {
                corRaio = Color.red;
            }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.DeteccaoChaoComprimento, corRaio);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.DeteccaoChaoComprimento, corRaio);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MoveStats.DeteccaoChaoComprimento), Vector2.right * boxCastSize.x, corRaio);
        }

        #endregion

    }

    private void BateuACabeca()
    {
        Vector2 boxCastOrigin = new Vector2(peCollisao.bounds.center.x, corpoCollisao.bounds.max.y);
        Vector2 boxCastSize = new Vector2(peCollisao.bounds.size.x * MoveStats.LarguraCabeca, MoveStats.DeteccaoCabecaComprimento);

        cabecaHitbox = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.DeteccaoCabecaComprimento, MoveStats.LayerChao);
        if(cabecaHitbox.collider != null)
        {
            bateuCabeca = true;
        }
        else
        {
            bateuCabeca = false;
        }

        #region Visualisação Debug
        if(MoveStats.DebugMostrarCabecaHitbox)
        {
            float LarguraCabeca = MoveStats.LarguraCabeca;

            Color raioCor;
            if(bateuCabeca)
            {
                raioCor = Color.green;
            }
            else
            {
                raioCor = Color.red;
            }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * LarguraCabeca, boxCastOrigin.y), Vector2.up * MoveStats.DeteccaoCabecaComprimento, raioCor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + (boxCastSize.x / 2) * LarguraCabeca, boxCastOrigin.y), Vector2.up * MoveStats.DeteccaoCabecaComprimento, raioCor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * LarguraCabeca, boxCastOrigin.y + MoveStats.DeteccaoCabecaComprimento), Vector2.right * boxCastSize.x * LarguraCabeca, raioCor);
        }

        #endregion
    }

    private void ChequeColisao()
    {
        noChao();
        BateuACabeca();
    }

    #endregion
    
    #region Timers

    private void ConteOsTimers()
    {
        tempoDeBufferDoPulo -= Time.deltaTime;

        if(!estaNoChao)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else 
        {
            coyoteTimer = MoveStats.CoyoteTime;
        }
    }

    #endregion
    

}
