using UnityEngine;

namespace Board {
    public class BoardCell : MonoBehaviour {
        
        public BoardController BoardController { get; set; }
        public CellTypes cellType;
        public int x;
        public int y;
        public int index;
    }
}