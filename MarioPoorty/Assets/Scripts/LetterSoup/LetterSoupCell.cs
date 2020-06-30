using LetterSoup.ScriptableObject;
using UnityEngine;

namespace LetterSoup {
    public class LetterSoupCell : MonoBehaviour {
        public Letters Letters { get; set; } = Letters.Null;

        private Sprite _currentSprite;
        public bool IsComplete { get; set; }
        public bool IsPartOfSolution { get; set; }

        public LetterSoupController letterSoupController;

        public bool _locked { get; set; }
        
        [SerializeField]
        private LettersSoupImageContainer lettersSoupImageContainer;

        private void OnMouseDown() {
            if(_locked) return;
            //gameObject.GetComponent<SpriteRenderer>().sprite = lettersSoupImageContainer.x;
            if (IsPartOfSolution) {
                IsComplete = true;
                GetComponent<SpriteRenderer>().color = Color.green;
                letterSoupController.CheckWordCompletion();
            }
        }

        public void UpdateChar(char c) {
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            switch (c) {
                case 'a':
                    spriteRenderer.sprite = lettersSoupImageContainer.a;
                    Letters = Letters.A;
                    break;
                case 'b':
                    spriteRenderer.sprite = lettersSoupImageContainer.b;
                    Letters = Letters.B;
                    break;
                case 'c':
                    spriteRenderer.sprite = lettersSoupImageContainer.c;
                    Letters = Letters.C;
                    break;
                case 'd':
                    spriteRenderer.sprite = lettersSoupImageContainer.d;
                    Letters = Letters.D;
                    break;
                case 'e':
                    spriteRenderer.sprite = lettersSoupImageContainer.e;
                    Letters = Letters.E;
                    break;
                case 'f':
                    spriteRenderer.sprite = lettersSoupImageContainer.f;
                    Letters = Letters.F;
                    break;
                case 'g':
                    spriteRenderer.sprite = lettersSoupImageContainer.g;
                    Letters = Letters.G;
                    break;
                case 'h':
                    spriteRenderer.sprite = lettersSoupImageContainer.h;
                    Letters = Letters.H;
                    break;
                case 'i':
                    spriteRenderer.sprite = lettersSoupImageContainer.i;
                    Letters = Letters.I;
                    break;
                case 'j':
                    spriteRenderer.sprite = lettersSoupImageContainer.j;
                    Letters = Letters.J;
                    break;
                case 'k':
                    spriteRenderer.sprite = lettersSoupImageContainer.k;
                    Letters = Letters.K;
                    break;
                case 'l':
                    spriteRenderer.sprite = lettersSoupImageContainer.l;
                    Letters = Letters.L;
                    break;
                case 'm':
                    spriteRenderer.sprite = lettersSoupImageContainer.m;
                    Letters = Letters.M;
                    break;
                case 'n':
                    spriteRenderer.sprite = lettersSoupImageContainer.n;
                    Letters = Letters.N;
                    break;
                case 'o':
                    spriteRenderer.sprite = lettersSoupImageContainer.o;
                    Letters = Letters.O;
                    break;
                case 'p':
                    spriteRenderer.sprite = lettersSoupImageContainer.p;
                    Letters = Letters.P;
                    break;
                case 'q':
                    spriteRenderer.sprite = lettersSoupImageContainer.q;
                    Letters = Letters.Q;
                    break;
                case 'r':
                    spriteRenderer.sprite = lettersSoupImageContainer.r;
                    Letters = Letters.R;
                    break;
                case 's':
                    spriteRenderer.sprite = lettersSoupImageContainer.s;
                    Letters = Letters.S;
                    break;
                case 't':
                    spriteRenderer.sprite = lettersSoupImageContainer.t;
                    Letters = Letters.T;
                    break;
                case 'u':
                    spriteRenderer.sprite = lettersSoupImageContainer.u;
                    Letters = Letters.U;
                    break;
                case 'v':
                    spriteRenderer.sprite = lettersSoupImageContainer.v;
                    Letters = Letters.V;
                    break;
                case 'w':
                    spriteRenderer.sprite = lettersSoupImageContainer.w;
                    Letters = Letters.W;
                    break;
                case 'x':
                    spriteRenderer.sprite = lettersSoupImageContainer.x;
                    Letters = Letters.X;
                    break;
                case 'y':
                    spriteRenderer.sprite = lettersSoupImageContainer.y;
                    Letters = Letters.Y;
                    break;
                case 'z':
                    spriteRenderer.sprite = lettersSoupImageContainer.z;
                    Letters = Letters.Z;
                    break;
                default:
                    Debug.Log("Default");
                    break;
            }
        }
    }
}