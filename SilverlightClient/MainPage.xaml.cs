using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SilverlightClient
{
	public partial class MainPage : UserControl
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void Log(string logMessage)
		{
			string time = DateTime.Now.ToString("hh:mm:ss.fff");
			Dispatcher.BeginInvoke(new Action(() => LogListBox.Items.Add(new ListBoxItem { Content = time + "|" + logMessage })));
		}

		private void Connect(object sender, RoutedEventArgs e)
		{
			Log("Connecting");
			Log("Ip " + IpAddressServer.SelectionBoxItem.ToString());
			Log("Port " + int.Parse(PortServer.SelectionBoxItem.ToString()));

			IPAddress destinationAddr = IPAddress.Parse(IpAddressServer.SelectionBoxItem.ToString());
			var socketEventArg = new SocketAsyncEventArgs();
			// Create a socket and connect to the server
			var sock = new Socket(destinationAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			socketEventArg.Completed += SocketEventArg_Completed;
			socketEventArg.RemoteEndPoint = new IPEndPoint(destinationAddr, int.Parse(PortServer.SelectionBoxItem.ToString()));
			socketEventArg.UserToken = sock;
			sock.ConnectAsync(socketEventArg);
		}

		/// <summary>
		/// A single callback is used for all socket operations. This method forwards execution on to the correct handler 
		/// based on the type of completed operation
		/// </summary>
		private void SocketEventArg_Completed(object sender, SocketAsyncEventArgs e)
		{
			switch (e.LastOperation)
			{
				case SocketAsyncOperation.Connect:
					ProcessConnect(e);
					break;
				case SocketAsyncOperation.Receive:
					ProcessReceive(e);
					break;
				case SocketAsyncOperation.Send:
					ProcessSend(e);
					break;
				default:
					throw new Exception("Invalid operation completed");
			}
		}

		/// <summary>
		/// Called when a ConnectAsync operation completes
		/// </summary>
		private void ProcessConnect(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				Log("Successfully connected to the server");

				// Send 'Hello World' to the server
				byte[] buffer = Encoding.UTF8.GetBytes("Hello World");
				e.SetBuffer(buffer, 0, buffer.Length);
				var sock = e.UserToken as Socket;
				bool willRaiseEvent = sock.SendAsync(e);
				if (!willRaiseEvent)
				{
					ProcessSend(e);
				}
			}
			else
			{
				throw new SocketException((int)e.SocketError);
			}
		}

		/// <summary>
		/// Called when a ReceiveAsync operation completes
		/// </summary>
		private void ProcessReceive(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				Log("Received from server: " + Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred));

				// Data has now been sent and received from the server. Disconnect from the server
				var sock = e.UserToken as Socket;
				sock.Shutdown(SocketShutdown.Send);
				sock.Close();
			}
			else
			{
				throw new SocketException((int)e.SocketError);
			}
		}


		/// <summary>
		/// Called when a SendAsync operation completes
		/// </summary>
		private void ProcessSend(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				Log("Sent 'Hello World' to the server");

				//Read data sent from the server
				var sock = e.UserToken as Socket;
				bool willRaiseEvent = sock.ReceiveAsync(e);
				if (!willRaiseEvent)
				{
					ProcessReceive(e);
				}
			}
			else
			{
				throw new SocketException((int)e.SocketError);
			}
		}
	}
}
