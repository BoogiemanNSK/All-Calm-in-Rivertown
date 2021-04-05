using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class InteractionTip : MonoBehaviour {

        [SerializeField] private GameObject _tipUIObject;
        [SerializeField] private Text _tipText;
        
        private void Awake() {
            HideTip("");
        }

        private void Start() {
            EventManager.StartListening(Constants.ShowTipEvent, ShowCorrectTip);
            EventManager.StartListening(Constants.HideTipEvent, HideTip);
        }

        private void ShowCorrectTip(string textToShow) {
            _tipUIObject.SetActive(true);
            _tipText.text = textToShow;
        }

        private void HideTip(string _) {
            _tipUIObject.SetActive(false);
        }

    }

}