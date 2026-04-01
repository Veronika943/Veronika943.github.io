using System;
using System.Windows.Forms;


namespace VectorEditor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());  // Если Form1 в папке Forms, нужно использовать полное имя
        }
    }
}