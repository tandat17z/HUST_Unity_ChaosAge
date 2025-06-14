using ChaosAge.data;
using DatSystem;
using DatSystem.UI;
using DatSystem.utils;
using UnityEngine;

namespace ChaosAge.manager
{
    public class CheatManager : Singleton<CheatManager>
    {

        protected override void OnAwake()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if(PanelManager.Instance.GetPanel<PanelCheat>() != null)
                {
                    PanelManager.Instance.ClosePanel<PanelCheat>();
                }
                else
                {
                    PanelManager.Instance.OpenPanel<PanelCheat>();
                }
            }
        }
    }

}

