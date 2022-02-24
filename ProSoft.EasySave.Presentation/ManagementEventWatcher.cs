using System;

namespace ProSoft.EasySave.Presentation
{
    internal class ManagementEventWatcher
    {
        private string query;

        public ManagementEventWatcher(string query)
        {
            this.query = query;
        }

        public Action<object, object> EventArrived { get; internal set; }

        internal void Start()
        {
            throw new NotImplementedException();
        }
    }
}