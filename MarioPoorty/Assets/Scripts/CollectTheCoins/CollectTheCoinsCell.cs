using UnityEngine;

namespace CollectTheCoins {
    public class CollectTheCoinsCell : MonoBehaviour {

        public int Value { get; set; }
        public bool Clicked { get; set; }
        public CollectTheCoinsController CollectTheCoinsController { get; set; }

        private void OnMouseDown() {
            if(Clicked) return;
            GetComponent<SpriteRenderer>().sprite = CollectTheCoinsController.imagesContainer.xImage;
            CollectTheCoinsController.AddCoinsValue(Value);
            Clicked = true;
        }
    }
}