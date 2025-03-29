using System.Collections.Generic;
using System.Collections;
using UnityEngine;

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
    private RaycastHit2D cabecaAtingida;
    private bool estaNoChao;
    private bool bateuCabeca;

    void Awake()
    {
        olhandoParaDireita = true;

        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        ChequeColisao();

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

    private void ChequeColisao()
    {
        noChao();
    }

    #endregion

}
