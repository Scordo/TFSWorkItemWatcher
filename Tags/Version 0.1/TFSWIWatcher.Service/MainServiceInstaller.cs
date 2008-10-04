using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace TFSWIWatcher.Service
{
    [RunInstaller(true)]
    public partial class MainServiceInstaller : Installer
    {
        public MainServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
