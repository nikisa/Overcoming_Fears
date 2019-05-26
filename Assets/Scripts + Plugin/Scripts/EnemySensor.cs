﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour {

    public Vector3 directionToSearch = new Vector3(0f, 0f, BoardManager.spacing);

    Node m_nodeToSearch;
    BoardManager m_board;
    
    Node m_previousEnemyNode;

    public Node PreviousEnemyNode { get { return m_previousEnemyNode; } set { m_previousEnemyNode = FindEnemyNode(); } }

    [SerializeField]
    public bool m_foundPlayer = false;

    public bool FoundPlayer { get { return m_foundPlayer; } set { m_foundPlayer = value; } }

	void Awake() {
        m_board = Object.FindObjectOfType<GameManager>().GetComponent<BoardManager>();
	}
	
    public void UpdateSensor(Node enemyNode) {
        Vector3 worldSpacePositionToSearch = transform.TransformVector(directionToSearch) + transform.position;

        if (m_board != null) {
            m_nodeToSearch = m_board.FindNodeAt(worldSpacePositionToSearch);
        
            if (!enemyNode.LinkedNodes.Contains(m_nodeToSearch)) {
                m_foundPlayer = false;
                return;
            }

            if (m_nodeToSearch == m_board.playerNode) {
                m_foundPlayer = true;
            }
            else
            {
                m_foundPlayer = false;
            }
        }
    }

    public Node FindEnemyNode() {
        return m_board.FindNodeAt(transform.position);
    }


    public void SetPreviousEnemyNode(Node n) {
        PreviousEnemyNode = n;
    }

    public Node GetPreviousEnemyNode() {
        return PreviousEnemyNode;
    }

    public void UpdateTriggerToFalse() {
        PreviousEnemyNode.triggerState = false;
    }
}
