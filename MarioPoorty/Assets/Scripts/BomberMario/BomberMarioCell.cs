using UnityEngine;
    
namespace BomberMario {
    public class BomberMarioCell : MonoBehaviour {

        public int I { get; set; }
        public int J { get; set; }

        public BomberMarioController BomberMarioController { get; set; }
        public bool IsTreasure { get; set; }
        public bool Locked { get; set; }
        private bool _discovered;
        

        private void OnMouseDown() {
            if (BomberMarioController.GetBombsLeft() <= 0) return;
            if(Locked) return;
            BomberMarioController.Bomb.ExplosionCalculate(BomberMarioController.Board, I, J);
            BomberMarioController.DecreaseBombCount();
        }
        
        public void ClearMark() {
            if (IsTreasure) {
                if (_discovered) return;
                
                _discovered = true;
                BomberMarioController.treasureLeft--;
                GetComponent<SpriteRenderer>().sprite = BomberMarioController.imagesContainer.xImage;
            }
            else {
                GetComponent<SpriteRenderer>().sprite = BomberMarioController.imagesContainer.emptyImage;
            }
        }
    }
    
}