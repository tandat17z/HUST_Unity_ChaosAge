using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DatSystem.utils;
using UnityEngine;

namespace DatSystem.UI
{
    public class PanelManager : Singleton<PanelManager>
    {
        private readonly List<View> _stackPanels = new();
        private readonly Dictionary<string, View> _cache = new();

        public T GetPanel<T>() where T : View
        {
            return (T)_stackPanels.Find(panel => panel.GetType().ToString().Equals(typeof(T).ToString()));
        }


        public Type CurrentPanelType => _stackPanels.Count > 0
            ? _stackPanels.Last().GetType()
            : null;

        public View GetCurrentPanel => _stackPanels.LastOrDefault();


        protected override void OnAwake()
        {
            //// check back button in Android by UniRX
            //Observable.EveryUpdate()
            //    .Where(_ => Input.GetKeyDown(KeyCode.Escape))
            //    .Subscribe(_ => TryCloseCurrentPanel())
            //    .AddTo(this);

            foreach (Transform panel in transform)
            {
                Destroy(panel.gameObject);
            }

        }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Escape))
                TryCloseCurrentPanel();
        }
#endif
        public void OpenForget<T>(UIData uiData = null) where T : View
        {
            OpenPanel<T>(uiData).Forget();
        }

        public async UniTask<T> OpenPanelByName<T>(string panelName, UIData uiData = null) where T : View
        {
            // create panel async
            var startFrameCount = Time.frameCount;
            if (_cache.TryGetValue(panelName, out var panel))
            {
                panel.transform.SetAsLastSibling();
                _cache.Remove(panelName);
            }
            else
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

#if using_addressable
                panel = (await Addressables.InstantiateAsync(panelName, transform)).GetComponent<T>();
#else
                Debug.Log($"UI/{panelName}");
                var panelPref = await Resources.LoadAsync($"UI/{panelName}") as GameObject;
                panel = Instantiate(panelPref, transform).GetComponent<T>();

#endif
                stopwatch.Stop();
                Debug.Log(
                    $"[PanelManager] Created {panelName.Color("lime")} in {stopwatch.ElapsedMilliseconds}ms (frame: {Time.frameCount - startFrameCount})");
                panel.Init();
            }

            panel.Open(uiData);
            Debug.Log("Open Panel: " + panelName);
            var currentDisplay = GetTopScreen();
            if (currentDisplay)
            {
                currentDisplay.OnFocusLost();
            }

            panel.OnFocus();
            _stackPanels.Insert(0, panel);

            return (T)panel;
        }

        public async UniTask<T> OpenPanel<T>(UIData uiData = null) where T : View
        {
            var panelName = typeof(T).ToString();
            return await OpenPanelByName<T>(panelName, uiData);
        }

        public async UniTask ClosePanel<T>(bool immediately = false, bool waitCloseCompleted = false) where T : View
        {
            string panelId = typeof(T).ToString();
            var panel = _stackPanels.Find(panel => panel.id.Equals(panelId));
            if (panel == null) return;

            // play close animation (if not immediately)
            if (immediately)
                panel.CloseImmediately();
            else
                panel.Close();

            // wait until close completed
            if (waitCloseCompleted)
                await UniTask.WaitUntil(() => panel == null);
        }


        public void ReleasePanel(View panelClosed)
        {
            Debug.Log("[PanelManager] Released " + panelClosed.id.ToString().Color("green"));
            panelClosed.OnFocusLost();
            _stackPanels.Remove(panelClosed);
            var newTopScreen = GetTopScreen();
            if (newTopScreen)
            {
                newTopScreen.OnFocus();
            }

            if (panelClosed.keepCached)
            {
                _cache.TryAdd(panelClosed.id, panelClosed);
                panelClosed.gameObject.SetActive(false);
                return;
            }

            Destroy(panelClosed.gameObject);
        }


        private void TryCloseCurrentPanel()
        {
            if (_stackPanels.Count == 0)
            {
                Debug.LogWarning("[PanelManager] Stack is empty");
                return;
            }

            //if (!_stackPanels.Last().CanBack)
            //{
            //	Debug.LogWarning("[PanelManager] Cannot back");
            //	return;
            //}

            Debug.Log("[PanelManager] Close " + CurrentPanelType.Name.Color("cyan"));
            var panelInTop = _stackPanels.First();
            _stackPanels.Remove(panelInTop);
            panelInTop.Close();
        }

        private View GetTopScreen()
        {
            return _stackPanels.FirstOrDefault();
        }
    }


}