namespace ChaosAge.UI.elements
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ButtonInfo : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] TMP_Text textName;

        void Start()
        {
            button.onClick.AddListener(OnClicked);
            //textName.text = buildingType.ToString();
        }

        private void OnClicked()
        {
            Debug.Log("ClickButtonInfo");
        }
    }

    public enum EButtonInfoType
    {

    }

}
