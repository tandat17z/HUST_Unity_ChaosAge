using System.Collections.Generic;
using DatSystem.UI;
using UnityEngine;

namespace DatSystem.UI
{
    public abstract class View : MonoBehaviour
    {
        public string id { get => this.GetType().ToString(); }
        public bool keepCached;
        protected UIData uiData;

        public void Init()
        {
            OnSetup();
        }

        public abstract void OnSetup();

        public abstract void Open(UIData uiData);
        public abstract void OnOpenCompleted();
        public abstract void Close();


        public abstract void OnFocus();
        public abstract void OnFocusLost();

        public virtual void CloseImmediately()
        {
            OnCloseCompleted();
        }

        protected virtual void OnCloseCompleted()
        {
            PanelManager.Instance.ReleasePanel(this);
        }
    }

}