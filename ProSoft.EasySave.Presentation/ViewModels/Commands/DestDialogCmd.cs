//using ProSoft.EasySave.Infrastructure.Models.Contexts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace ProSoft.EasySave.Presentation.ViewModels.Commands
//{
//    internal class DestDialogCmd : CommandBase
//    {
//        private JobContext _jobContext;

//        public DestDialogCmd(JobContext jobContext)
//        {
//            jobContext = _jobContext;
//        }

//        public override void Execute(object parameter)
//        {
//            var openfolder = new FolderBrowserDialog();
//            openfolder.ShowDialog();
//            _jobContext.DestinationPath = openfolder.SelectedPath;

//        }
//    }
//}