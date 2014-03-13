using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SilverlightPolicyServer
{
	class LoggerListBox : ILogger
	{
		public LoggerListBox(ListBox lb)
		{
			_lb = lb;
		}

		private ListBox _lb;

		public void Log(string logMessage)
		{
			string time = DateTime.Now.ToString("hh:mm:ss.fff");
			Application.Current.Dispatcher.BeginInvoke(new Action(() => _lb.Items.Add(new ListBoxItem { Content = time + "|" + logMessage })));
		}
	}
}
