using System;
using System.Windows;

namespace GodotLocalizationEditor.Widgets
{
    public partial class AskKey : Window
    {
        public AskKey()
        {
            InitializeComponent();
        }

        void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        
        void Window_ContentRendered(object sender, EventArgs e)
        {
            TxtAnswer.SelectAll();
            TxtAnswer.Focus();
        }

        public string Answer => TxtAnswer.Text;
    }
}

