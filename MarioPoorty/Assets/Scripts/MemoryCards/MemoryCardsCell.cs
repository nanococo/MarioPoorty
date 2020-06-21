using System;
using UnityEngine;

namespace MemoryCards {
    public class MemoryCardsCell : MonoBehaviour {

        public int Id { get; set; }
        public MemoryCardsController MemoryCardsController { get; set; }

        private void OnMouseDown() {
            if (MemoryCardsController.matchedCards.Contains(Id)) return;
            
            GetComponent<SpriteRenderer>().sprite = MemoryCardsController.GetImageById(Id);
            MemoryCardsController.CheckPair(this);
        }
    }
}