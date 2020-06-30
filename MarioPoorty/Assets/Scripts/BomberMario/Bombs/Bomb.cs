using System;
using UnityEngine;

namespace BomberMario.Bombs {
    public abstract class Bomb {
        protected int Size;
        public BombType BombType;

        protected Bomb(BombType bombType) {
            BombType = bombType;
        }
        
        
        public abstract void ExplosionCalculate(GameObject[,] board, int i, int j);
    }
}