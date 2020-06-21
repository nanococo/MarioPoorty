using System;
using UnityEngine;

namespace BomberMario.Bombs {
    public abstract class Bomb {

        protected readonly Sprite ImageClear;
        protected int Size;
        public BombType BombType;

        protected Bomb(Sprite sprite, BombType bombType) {
            ImageClear = sprite;
            BombType = bombType;
        }
        
        
        public abstract void ExplosionCalculate(GameObject[,] board, int i, int j);
    }
}