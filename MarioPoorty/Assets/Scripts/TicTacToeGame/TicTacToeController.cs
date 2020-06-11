using System;
using UnityEngine;

 namespace TicTacToeGame {
    public class TicTacToeController : MonoBehaviour {

        public GameObject cellPrefab;
        private GameObject[,] _grid = new GameObject[3,3];
        public TicTacToeMarks CurrentMark { private set; get; } = TicTacToeMarks.XMark;

        private void Start() {
            
            var y = 2;
            for (var i=0;i<3;i++) {
                var x = -2;
                for (var j=0;j<3;j++) {
                    var newCell = Instantiate(cellPrefab, transform);
                    var newTransform = newCell.GetComponent<Transform>();
                    newTransform.position = new Vector2(x,y);
                    x += 2;
                    newCell.GetComponent<Cell>().TicTacToeController = this;
                    _grid[i, j] = newCell;
                }
                y -= 2;
            }
        }

        public bool CheckWin() {


            for (var lines=0; lines<3; lines++) {
                if (CheckRow(lines)) {
                    return true;
                }

                if (CheckColumn(lines)) {
                    return true;
                }
            }
            return CheckDiagonals();
            
            // var res = "";
            // for (var i = 0; i < 3; i++) {
            //     for (var j = 0; j < 3; j++) {
            //         res+= _grid[i,j].GetComponent<Cell>().CellMark+", ";
            //     }
            //
            //     res += "\n";
            // }
            // Debug.Log(res);
        }

        private bool CheckColumn(int j) {
            var compare =  _grid[0, j].GetComponent<Cell>().CellMark;
            for (int i = 0; i < 3; i++) {
                var current = _grid[i, j].GetComponent<Cell>().CellMark;
                if (current == TicTacToeMarks.Empty) {
                    return false;
                }

                if (current==compare) {
                    compare = current;
                }
                else {
                    return false;
                }
            }
            return true;
        }

        private bool CheckRow(int i) {
            var compare =  _grid[i, 0].GetComponent<Cell>().CellMark;
            for (var j = 0; j < 3; j++) {
                var current = _grid[i, j].GetComponent<Cell>().CellMark;
                if (current == TicTacToeMarks.Empty) {
                    return false;
                }

                if (current==compare) {
                    compare = current;
                }
                else {
                    return false;
                }
            }
            return true;
        }

        private bool CheckDiagonals() {
            if (_grid[0, 0].GetComponent<Cell>().CellMark == _grid[1, 1].GetComponent<Cell>().CellMark &&
                _grid[1, 1].GetComponent<Cell>().CellMark == _grid[2, 2].GetComponent<Cell>().CellMark) {
                return _grid[0, 0].GetComponent<Cell>().CellMark != TicTacToeMarks.Empty;
            }
            
            return _grid[0, 2].GetComponent<Cell>().CellMark == _grid[1, 1].GetComponent<Cell>().CellMark &&
                   _grid[1, 1].GetComponent<Cell>().CellMark == _grid[2, 0].GetComponent<Cell>().CellMark && 
                   _grid[0, 2].GetComponent<Cell>().CellMark != TicTacToeMarks.Empty;
        }
        
        public void TickTurn() {
            if (CurrentMark == TicTacToeMarks.XMark) {
                CurrentMark = TicTacToeMarks.Circle;
            }
            else {
                CurrentMark = TicTacToeMarks.XMark;
            }
        }
    }
 }