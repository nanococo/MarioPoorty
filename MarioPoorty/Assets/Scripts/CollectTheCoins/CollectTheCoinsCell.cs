using System;
using UnityEngine;

namespace CollectTheCoins {
    public class CollectTheCoinsCell : MonoBehaviour {

        public int Value { get; set; }
        private bool _clicked = false;
        public CollectTheCoinsController CollectTheCoinsController { get; set; }

        private void OnMouseDown() {
            if(_clicked) return;
            
            CollectTheCoinsController.AddCoinsValue(Value);
            _clicked = true;
        }
    }
}