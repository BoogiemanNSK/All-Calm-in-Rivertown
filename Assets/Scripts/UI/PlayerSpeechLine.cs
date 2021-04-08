using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class PlayerSpeechLine : MonoBehaviour {

        [SerializeField] private Text SpeechLine;

        private DialogScreen _parent;
        private int _lineIndex;
        
        public void SetSpeechLine(string text, int index, DialogScreen parent) {
            SpeechLine.text = text;
            _lineIndex = index;
            _parent = parent;
        }
        
        public void OnClick() {
            _parent.OnDialogLineClick(_lineIndex);
        }

    }

}