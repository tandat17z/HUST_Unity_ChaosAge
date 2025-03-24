namespace ChaosAge.UI.elements
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ButtonInfo : MonoBehaviour
    {
        [SerializeField] EButtonInfoType type;
        [SerializeField] Button button;
        [SerializeField] TMP_Text textName;

        [SerializeField] BaseActionButton actionButton;

        void Start()
        {
            button.onClick.AddListener(OnClicked);
            textName.text = type.ToString();
        }

        private void OnClicked()
        {
            actionButton.Active();
        }
    }

    [Serializable]
    public enum EButtonInfoType
    {
        Info,
        Upgrade
    }

}
