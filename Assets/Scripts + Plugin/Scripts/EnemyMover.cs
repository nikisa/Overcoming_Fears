﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// DUE ENEMIES COMPENETRANO TRA LORO SE UNO VIENE SPINTO NELLA TRAIETTORIA DELL'ALTRO
/// </summary>


public enum MovementType
{
    Stationary,
    Patrol,
    Spinner,
    Chaser,
    Boss,
}

public class EnemyMover : Mover
{

    public Vector3 directionToMove = new Vector3(0f, 0f, BoardManager.spacing);
    //public Vector3 directionBossToMove = new Vector3(0f, 0f, BoardManager.BossSpacinc);
    public MovementType firstMovementType = MovementType.Stationary;
    public MovementType movementType = MovementType.Stationary;


    public float standTime = 0f;

    PlayerManager m_player;

    [HideInInspector] public Vector3 firstDest;
    [HideInInspector] public Vector3 spottedDest;

    
    public int index;

    [HideInInspector] public bool firstChaserMove = false;

    public bool spottedPlayer;

    ArrayList playerPositions;


    protected override void Awake()
    {
        base.Awake();
        faceDestination = true;
        movementType = firstMovementType;
        m_player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        playerPositions = new ArrayList();
    }

    protected override void Start()
    {
        base.Start();
        spottedPlayer = false;
    }

    public void MoveOneTurn()
    {

        switch (movementType)
        {
            case MovementType.Patrol:
                Patrol();
                break;
            case MovementType.Stationary:
                Stand();
                break;
            case MovementType.Spinner:
                Spin();
                break;
            case MovementType.Chaser:
                Chase();
                break;
            case MovementType.Boss:
                Boss();
                break;
            default:
                Debug.Log("retard non hai messo il MoventType");
                break;
        }
    }

    void Chase()
    {
        StartCoroutine(ChaseRoutine());
    }

    IEnumerator ChaseRoutine()
    {

        Vector3 startPos = new Vector3(m_currentNode.Coordinate.x, 0f, m_currentNode.Coordinate.y);

        firstDest = startPos + transform.TransformVector(directionToMove);

        spottedDest = startPos + transform.TransformVector(directionToMove * 2f);

        if (m_board.playerNode == m_board.FindNodeAt(spottedDest) && !spottedPlayer && m_board.FindNodeAt(firstDest).LinkedNodes.Contains(m_board.FindNodeAt(spottedDest)))
        {

            Debug.Log("Spotted!");
            clearPlayerPath();
            m_board.ChasingPreviousPlayerNode = m_board.playerNode; //Cambiare PreviousPlayerNode , qui o su Board
            //Move(firstDest , 0f);
            spottedPlayer = true;

            UpdatePlayerPath();

        }

        else if (spottedPlayer)
        {

            UpdatePlayerPath();

            if (firstChaserMove == false)
            { // && CASELLA SUCCESSIVA NON è OCCUPATA (post armature)
                Move(firstDest, 0f);
                firstChaserMove = true;
            }
            else
            { // && CASELLA SUCCESSIVA NON è OCCUPATA (post armature)
                Debug.Log("Chasing...");

                //m_board.ChaserNewDest = m_board.ChasingPreviousPlayerNode;
                //m_board.ChasingPreviousPlayerNode = m_board.playerNode;

                //Debug.Log(m_board.ChasingPreviousPlayerNode);

                if (m_player.transform.position != firstDest)
                {

                    //Debug.Log(m_player.GetPlayerPath(index));

                    Move(GetPlayerPath(index).transform.position, 0f);

                    yield return new WaitForSeconds(0.6f);

                    destination = GetPlayerPath(index + 1).transform.position;
                    FaceDestination();


                }

                index++;
            }
        }




        while (isMoving)
        {
            yield return null;
        }

        base.finishMovementEvent.Invoke();
    }

    public int TEMP = 0;
    public void Boss() {
        StartCoroutine(BossRoutine());
    }

    IEnumerator BossRoutine() {
        Vector3 startPos = new Vector3(m_currentNode.Coordinate.x, 0f, m_currentNode.Coordinate.y);

        //One space forward
        Vector3 newDest = startPos + transform.TransformVector(directionToMove * 2f);

        //Two space forward
        Vector3 nextDest = startPos + transform.TransformVector(directionToMove * 4f);

        //COntrollare il movimento

        if (m_board != null) {
            Node newDestNode = m_board.FindNodeAt(newDest);
            Node nextDestNode = m_board.FindNodeAt(nextDest);


            if (newDestNode == null || newDestNode.LinkedNodes.Contains(newDestNode) || m_board.FindMovableObjectsAt(newDestNode).Count != 0 || m_board.FindSwordsAt(newDestNode).Count != 0 || (m_board.playerNode == newDestNode && spottedPlayer)) {

                Spin();

                yield return new WaitForSeconds(rotateTime);
            }
            else {
                Move(newDest, 0f);


                while (isMoving) {
                    yield return null;
                }
            }


            if (nextDestNode == null || nextDestNode.LinkedNodes.Contains(nextDestNode) || m_board.FindMovableObjectsAt(nextDestNode).Count != 0 || m_board.FindSwordsAt(nextDestNode).Count != 0 || (m_board.playerNode == nextDestNode && spottedPlayer)) {

                //SPOSTARE MOVIMENTO QUI DENTRO ALTRIMENTI SI BLOCCA NELL ANGOLINO E NON SI GIRA

                destination = startPos;
                FaceDestination();

                yield return new WaitForSeconds(rotateTime);
            }

            base.finishMovementEvent.Invoke();
        }


    }

    //public void bossMove() {
    //    Vector3 startPos = new Vector3(m_currentNode.Coordinate.x, 0f, m_currentNode.Coordinate.y);
    //    //One space forward
    //    Vector3 newDest = startPos + transform.TransformVector(directionToMove * 2f);

    //    //Controllare il movimento
    //    if (m_board != null) {
    //        Move(newDest, 0f);                
    //        }
    //    }



    void Patrol()
    {
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        Vector3 startPos = new Vector3(m_currentNode.Coordinate.x, 0f, m_currentNode.Coordinate.y);

        //One space forward
        Vector3 newDest = startPos + transform.TransformVector(directionToMove);

        //Two space forward
        Vector3 nextDest = startPos + transform.TransformVector(directionToMove * 2f);

        //COntrollare il movimento

        if (m_board != null)
        {
            Node newDestNode = m_board.FindNodeAt(newDest);
            Node nextDestNode = m_board.FindNodeAt(nextDest);

            if (newDestNode == null || newDestNode.LinkedNodes.Contains(newDestNode) || m_board.FindMovableObjectsAt(newDestNode).Count != 0 || m_board.FindSwordsAt(newDestNode).Count != 0 || (m_board.playerNode == newDestNode && spottedPlayer))
            {

                Spin();

                yield return new WaitForSeconds(rotateTime);
            }
            else
            {
                Move(newDest, 0f);

                while (isMoving)
                {
                    yield return null;
                }
            }


            if (nextDestNode == null || nextDestNode.LinkedNodes.Contains(nextDestNode) || m_board.FindMovableObjectsAt(nextDestNode).Count != 0 || m_board.FindSwordsAt(nextDestNode).Count != 0 || (m_board.playerNode == nextDestNode && spottedPlayer))
            {

                //SPOSTARE MOVIMENTO QUI DENTRO ALTRIMENTI SI BLOCCA NELL ANGOLINO E NON SI GIRA

                destination = startPos;
                FaceDestination();

                yield return new WaitForSeconds(rotateTime);
            }

            base.finishMovementEvent.Invoke();
        }
    }

    void Stand()
    {
        StartCoroutine(StandRoutine());
    }

    IEnumerator StandRoutine()
    {
        yield return new WaitForSeconds(standTime);
        base.finishMovementEvent.Invoke();
    }

    public void Spin()
    {
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        Vector3 localForward = new Vector3(0f, 0f, BoardManager.spacing);
        destination = transform.TransformVector(localForward * -1f) + transform.position;

        FaceDestination();

        yield return new WaitForSeconds(rotateTime);

        base.finishMovementEvent.Invoke();
    }


    public void UpdatePlayerPath() {
        playerPositions.Add(m_board.playerNode);

    }

    public Node GetPlayerPath(int i) {
        Node playerNode = (Node)playerPositions[i];
        return playerNode;
    }

    public void clearPlayerPath() {
        playerPositions.Clear();
        index = 0;
    }


}

