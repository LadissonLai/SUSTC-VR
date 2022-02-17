using Doozy.Engine.UI;
using Framework;
using Fxb.CMSVR;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

namespace Fxb.CPTTS
{
    public class StructureView : PadViewBase
    {
        public Toggle switchOn;

        public Toggle switchOff;

        public Image pic;

        public SpriteAtlas spriteAtlas;


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            switchOn.onValueChanged.AddListener(open);

            switchOff.onValueChanged.AddListener(close);
        }

        private void open(bool value)
        {
            if (true) 
            {
                pic.sprite = spriteAtlas.GetSprite("baozha");
            }
        }

        private void close(bool value)
        {
            if (true) 
            {
                pic.sprite = spriteAtlas.GetSprite("zuhe");
            }
        }
    }
}