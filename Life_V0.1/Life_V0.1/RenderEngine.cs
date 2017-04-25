using System.Drawing;
using System.Windows.Forms;

namespace RenderEngine_1._0
{
    public partial class RenderEngine
    {

        // This class should give flicker free graphics. It is slow, but "works".

        /// <summary>
        /// Global buffer, it stores current graphics
        /// </summary>
        public BufferedGraphics buffer;

        /// <summary>
        /// Targeted form, here everthing will be displayed
        /// </summary>
        Form target;

        /// <summary>
        /// Initialises "Render Engine"
        /// </summary>
        /// <param name="TargetForm">Targeted Form</param>
        public RenderEngine(Form TargetForm)
        {
            target = TargetForm;
            UpdateGraphicsBuffer();
        }

        /// <summary>
        /// Updates curren buffer size
        /// </summary>
        public void UpdateGraphicsBuffer()
        {
            if (target.Width > 0 && target.Height > 0)
            {
                Rectangle bufBoundry = new Rectangle(0, 0, target.Width, target.Height);
                BufferedGraphicsContext context = BufferedGraphicsManager.Current;
                buffer = context.Allocate(target.CreateGraphics(), bufBoundry);
            }
        }

        /// <summary>
        /// Fills buffer with black color
        /// </summary>
        public void ClearBuffer()
        {
            buffer.Graphics.Clear(Color.Black);
        }

        /// <summary>
        /// Fills buffer with given color
        /// </summary>
        /// <param name="fill">Color, which will fill the buffer</param>
        public void ClearBuffer(Color fill)
        {
            buffer.Graphics.Clear(fill);
        }

        /// <summary>
        /// Renders buffer to given graphics object
        /// </summary>
        /// <param name="targetGraphics">Obiekt grafiki</param>
        public void RenderBuffer(Graphics targetGraphics)
        {
            buffer.Render(targetGraphics);
        }
    }
}
