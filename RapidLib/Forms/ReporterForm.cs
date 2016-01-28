using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace RapidLib.Forms
{
    public partial class ReporterForm : Form
    {
        public ReporterForm()
        {
            InitializeComponent();
        }





        #region Resizing and Moving of Borederless Form
        protected override void WndProc(ref Message m)
        {
            const int wmNcHitTest = 0x84;
            const int htBottomLeft = 16;
            const int htBottomRight = 17;
            const int htCaption = 0x2;
            if (m.Msg == wmNcHitTest)
            {
                var x = (int)(m.LParam.ToInt64() & 0xFFFF);
                var y = (int)((m.LParam.ToInt64() & 0xFFFF0000) >> 16);
                var pt = PointToClient(new Point(x, y));
                var clientSize = ClientSize;
                if (pt.X >= clientSize.Width - 12 && pt.Y >= clientSize.Height - 12 && clientSize.Height >= 12)
                {
                    m.Result = (IntPtr)(IsMirrored ? htBottomLeft : htBottomRight); // resizable
                    return;
                }
                
            }
            base.WndProc(ref m);
            if (m.Msg == wmNcHitTest) m.Result = (IntPtr)(htCaption); // movable
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawGripper(e);
        }

        public void DrawGripper(PaintEventArgs e)
        {
            if (!VisualStyleRenderer.IsElementDefined(VisualStyleElement.Status.Gripper.Normal)) return;
            var renderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal);
            var rectangle1 = new Rectangle((Width) - 18, (Height) - 20, 20, 20);
            renderer.DrawBackground(e.Graphics, rectangle1);
        }

        private void ReporterForm_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void ReporterForm_Move(object sender, EventArgs e)
        {
            Invalidate();
        }
        #endregion Resizing and Moving of Borederless Form
    }
}
