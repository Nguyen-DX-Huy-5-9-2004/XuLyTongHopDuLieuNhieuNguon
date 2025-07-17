using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.LookAndFeel;

namespace nckhTGF {
    static class Program {
        public static bool IsRestarting = false;
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            do
            {
                IsRestarting = false;
                getData mainForm = new getData();
                Application.Run(mainForm);
            }
            while (IsRestarting);
        }

        public static void RestartApp()
        {
            IsRestarting = true;
            Application.Exit();
        }
    }
}