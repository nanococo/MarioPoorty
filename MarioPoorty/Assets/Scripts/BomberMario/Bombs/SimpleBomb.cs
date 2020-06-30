using UnityEngine;

namespace BomberMario.Bombs {
    public class SimpleBomb : Bomb {

        public SimpleBomb(BombType bombType) : base(bombType) {
        }
        
        public override void ExplosionCalculate(GameObject[,] board, int i, int j) {
            board[i, j].GetComponent<BomberMarioCell>().ClearMark();
        }
    }
}