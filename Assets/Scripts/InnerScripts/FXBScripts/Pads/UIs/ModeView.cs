using Doozy.Engine.UI;
using Framework;
using Fxb.CMSVR;
using UnityEngine;
using UnityEngine.UI;

namespace Fxb.CPTTS
{
    public class ModeView : PadViewBase
    {
        public Button[] menuMapBtns;

        public UIView homeView;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            for (var i = 0; i < menuMapBtns.Length; i++)
            {
                var btn = menuMapBtns[i];

                btn.onClick.AddListener(() =>
                {
                    EntrySetting.Instance.runMode = (Enums.RunMode)i;
                    switch (i)
                    {
                        case 0:
                            World.Get<Fxb.CMSVR.DASceneState>().taskMode = DaTaskMode.Teching;
                            break;
                        case 1:
                            World.Get<Fxb.CMSVR.DASceneState>().taskMode = DaTaskMode.Training;
                            break;
                        case 2:
                            World.Get<Fxb.CMSVR.DASceneState>().taskMode = DaTaskMode.Examination;
                            break;
                    }
                    Debug.Log("model choose press");
                    homeView.Show();
                });
            }

            doozyView.Show(true);
        }
    }
}