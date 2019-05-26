﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

    BoardManager m_board;

    protected Node m_currentNode;
    public Node CurrentNode { get { return m_currentNode; } }

    private void Awake() {
        m_board = Object.FindObjectOfType<GameManager>().GetComponent<BoardManager>();
    }

    private void Start() {
        m_currentNode = m_board.FindNodeAt(transform.position);
    }

    public Node FindSwordNode() {
        return m_board.FindNodeAt(transform.position);
    }


    //public void CaptureEnemies() {
    //    if (m_board != null) {
    //        List<EnemyManager> enemies = m_board.FindEnemiesAt(m_board.FindNodeAt(transform.position));
    //        Debug.Log(m_board.FindNodeAt(transform.position));

    //        //Enemies rimane vuoto

    //        if (enemies.Count != 0 ) {

    //            foreach (EnemyManager enemy in enemies) {
    //                if (enemy != null) {
    //                    enemy.Die();
    //                }
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// PATCH MOMENTANEA
    /// </summary>

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy") {
            other.GetComponent<EnemyManager>().Die();
        }
    }


}
