using Doozy.Engine.UI;
using Framework;
using Fxb.CMSVR;
using UnityEngine;
using UnityEngine.UI;

namespace Fxb.CPTTS
{
    public class HomeView : PadViewBase
    {
        public Button[] menuMapBtns;

        public UIView[] menuMapViews;

        public Button recordBtn;

        public Button taskDetailBtn;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            Debug.Assert(menuMapBtns.Length == menuMapViews.Length);

            for (var i = 0; i < menuMapBtns.Length; i++)
            {
                var btn = menuMapBtns[i];

                var _i = i;

                btn.onClick.AddListener(() =>
                {
                    menuMapViews[_i].Show();
                });
            }

            doozyView.Show(true);
        }

        public void TryExchangeTaskBtn()
        {
            if (World.Get<ITaskModel>() != null)
                taskDetailBtn.gameObject.SetActive(true);
        }

        public void TryExchangeRecordBtn()
        {
            var task = World.Get<ITaskModel>();

            if (task != null && task.IsSubmitAllTask)
                recordBtn.gameObject.SetActive(true);
        }
    }
}