using UnityEngine;

namespace BomberMario.Bombs {
    public class LineBomb : Bomb{
        public LineBomb(Sprite sprite, BombType bombType) : base(sprite, bombType) {
        }

        public override void ExplosionCalculate(GameObject[,] board, int i, int j) {
            Size = board.GetLength(0);
            if (j==0) {
                board[i, j].GetComponent<BomberMarioCell>().ClearMark();
                board[i, j+1].GetComponent<BomberMarioCell>().ClearMark();
                board[i, j+2].GetComponent<BomberMarioCell>().ClearMark();
                board[i, j+3].GetComponent<BomberMarioCell>().ClearMark();
            } else if (j == Size - 1) {
                board[i, j].GetComponent<BomberMarioCell>().ClearMark();
                board[i, j-1].GetComponent<BomberMarioCell>().ClearMark();
                board[i, j-2].GetComponent<BomberMarioCell>().ClearMark();
                board[i, j-3].GetComponent<BomberMarioCell>().ClearMark();
            } else if (j == Size - 2) {
                board[i, j].GetComponent<BomberMarioCell>().ClearMark();
                board[i, j+1].GetComponent<BomberMarioCell>().ClearMark();
                board[i, j-1].GetComponent<BomberMarioCell>().ClearMark();
                board[i, j-2].GetComponent<BomberMarioCell>().ClearMark();   
            }
            else {
                board[i, j].GetComponent<BomberMarioCell>().ClearMark();
                board[i, j-1].GetComponent<BomberMarioCell>().ClearMark();
                board[i, j+1].GetComponent<BomberMarioCell>().ClearMark();
                board[i, j+2].GetComponent<BomberMarioCell>().ClearMark();
            }
        }
    }
}