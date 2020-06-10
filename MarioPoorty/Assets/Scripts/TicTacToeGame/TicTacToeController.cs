﻿using System;
 using System.Collections.Generic;
 using UnityEngine;

 namespace TicTacToeGame {
    public class TicTacToeController : MonoBehaviour {

        public GameObject cellPrefab;

        private List<GameObject> _cells = new List<GameObject>();

        private void Start() {

            var row = 2;
            for (var i=0;i<3;i++) {
                var col = -2;
                for (var j=0;j<3;j++) {
                    var newCell = Instantiate(cellPrefab, transform);
                    var newTransform = newCell.GetComponent<Transform>();
                    newTransform.position = new Vector2(row,col);
                    col += 2;
                    _cells.Add(newCell);
                }
                row -= 2;
            }
        }
    }
 }