using System.Windows.Forms;

namespace RapidLib
{
    public class TransparentLabel : Label
    {
        public TransparentLabel()
        {
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.ExStyle |= 0x20;  // Turn on WS_EX_TRANSPARENT
                return parms;
            }
        }
    }
}