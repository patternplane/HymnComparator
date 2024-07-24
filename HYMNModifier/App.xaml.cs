using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HYMNModifier
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void EH_ApplicationStartup(object sender, StartupEventArgs e)
        {
            new MainWindow(new viewmodel.VMMainWindow()).Show();
        }
    }
}
