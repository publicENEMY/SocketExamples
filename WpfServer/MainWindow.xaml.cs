using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using AsyncSocketServer;
using SilverlightPolicyServer;

namespace WpfServer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		static ListBox logListBox = new ListBox();
		LoggerListBox _logListBox = new LoggerListBox(logListBox);

		private Server server;

		public MainWindow()
		{

			InitializeComponent();

			logListBox.Margin = new Thickness(10);
			MainGrid.Children.Add(logListBox);
			
			// start policy server
			var ps = new PolicyServer("clientaccesspolicy.xml", _logListBox);

			// start server
			StartServer();
		}

		private void StartServer()
		{
			//# Connections: The maximum number of connections the server will accept simultaneously.
			//Receive Size in Bytes: The buffer size used by the server for each receive operation.  
			//Address family: The address family of the socket the server will use to listen for incoming connections.  Supported values are ‘ipv4’ and ‘ipv6’.
			//Local Port Number: The port the server will bind to.

			int numConnections = 1000;
			int receiveSize = 1024;
			var localEndPoint = new IPEndPoint(IPAddress.Any, 4502);

			// Start the server listening for incoming connection requests
			server = new Server(numConnections, receiveSize, _logListBox);
			server.Init();
			server.Start(localEndPoint);
		}
	}
}
