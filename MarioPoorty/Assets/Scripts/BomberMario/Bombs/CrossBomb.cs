using UnityEngine;

namespace BomberMario.Bombs {
    public class CrossBomb : Bomb {

        public CrossBomb(BombType bombType) : base(bombType) {
        }
        
        public override void ExplosionCalculate(GameObject[,] board, int i, int j) {
            Size = board.GetLength(0);

            if (i == 0) {
                if (j == Size - 1) {
                    board[i, j].GetComponent<BomberMarioCell>().ClearMark();
                    board[i+1, j].GetComponent<BomberMarioCell>().ClearMark();
                    board[i, j - 1].GetComponent<BomberMarioCell>().ClearMark();
                } else if (j == 0) {
                    board[i, j].GetComponent<BomberMarioCell>().ClearMark();
                    board[i+1, j].GetComponent<BomberMarioCell>().ClearMark();
                    board[i, j + 1].GetComponent<BomberMarioCell>().ClearMark();
                }
                else {
                    board[i, j].GetComponent<BomberMarioCell>().ClearMark();
                    board[i, j+1].GetComponent<BomberMarioCell>().ClearMark();
                    board[i, j-1].GetComponent<BomberMarioCell>().ClearMark();
                    board[i+1, j].GetComponent<BomberMarioCell>().ClearMark();
                }
            } else if (j == Size - 1) {
                if (i == Size - 1) {
                    board[i, j].GetComponent<BomberMarioCell>().ClearMark();
                    board[i-1, j].GetComponent<BomberMarioCell>().ClearMark();
                    board[i, j - 1].GetComponent<BomberMarioCell>().ClearMark();
                }
                else {
                    board[i, j].GetComponent<BomberMarioCell>().ClearMark();
                    board[i - 1, j].GetComponent<BomberMarioCell>().ClearMark();
                    board[i, j - 1].GetComponent<BomberMarioCell>().ClearMark();
                    board[i + 1, j].GetComponent<BomberMarioCell>().ClearMark();    
                }
            } else if (i == Size - 1) {
                if (j==0) {
                    board[i, j].GetComponent<BomberMarioCell>().ClearMark();
                    board[i-1, j].GetComponent<BomberMarioCell>().ClearMark();
                    board[i, j + 1].GetComponent<BomberMarioCell>().ClearMark();
                }
                else {
                    board[i, j].GetComponent<BomberMarioCell>().ClearMark();
                    board[i-1, j].GetComponent<BomberMarioCell>().ClearMark();
                    board[i, j - 1].GetComponent<BomberMarioCell>().ClearMark();
                    board[i, j + 1].GetComponent<BomberMarioCell>().ClearMark();    
                }
                
            }
            else {
                board[i, j].GetComponent<BomberMarioCell>().ClearMark();
                board[i-1, j].GetComponent<BomberMarioCell>().ClearMark();
                board[i+1, j].GetComponent<BomberMarioCell>().ClearMark();
                board[i, j+1].GetComponent<BomberMarioCell>().ClearMark();
                board[i, j-1].GetComponent<BomberMarioCell>().ClearMark();
            }
        }

        
    }
}