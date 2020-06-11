using UnityEngine;

namespace TicTacToeGame {
    
    [CreateAssetMenu(menuName = "TicTacToe Shared Container")]
    public class ImagesContainer : ScriptableObject {
        public Sprite xImage;
        public Sprite emptyImage;
        public Sprite circleImage;
    }
}