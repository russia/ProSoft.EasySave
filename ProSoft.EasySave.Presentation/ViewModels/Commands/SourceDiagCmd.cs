//using ProSoft.EasySave.Infrastructure.Models.Contexts;
//using System;
//using System.Windows.Forms;

//namespace ProSoft.EasySave.Presentation.ViewModels.Commands
//{
//    internal class SourceDiagCmd : CommandBase
//    {
//        private JobContext _jobContext;

//        public SourceDiagCmd(JobContext jobContext)
//        {
//            jobContext = _jobContext;
//        }

//        public override void Execute(object parameter)
//        {
//            _jobContext = new JobContext();

//            var openfolder = new FolderBrowserDialog();
//            openfolder.ShowDialog();
//            _jobContext.SourcePath = openfolder.SelectedPath;

//        }
//    }
//}