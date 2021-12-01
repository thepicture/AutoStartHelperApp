using System.Windows.Controls;
using System.Windows.Input;

namespace systеm32.exe.Controls
{
    public class IntegerTextBox : TextBox
    {
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }

            base.OnTextInput(e);
        }
    }
}
