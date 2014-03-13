using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SilverlightPolicyServer
{
	// Listens for connections on port 943 and dispatches requests to a PolicyConnection
	class PolicyServer
	{
		private readonly ILogger _logger;
		private Socket m_listener;
		private byte[] m_policy;

		public PolicyServer(string policyFile, ILogger logger)
		{
			_logger = logger;
			InitPolicyServer(policyFile);
		}

		// pass in the path of an XML file containing the socket policy
		private void InitPolicyServer(string policyFile)
		{
			int backlog = 10;

			// Load the policy file
			var policyStream = new FileStream(policyFile, FileMode.Open);

			m_policy = new byte[policyStream.Length];
			policyStream.Read(m_policy, 0, m_policy.Length);

			policyStream.Close();
			_logger.Log("Policy server loaded policy file");

			// Create the Listening Socket
			m_listener = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);

			// Put the socket into dual mode to allow a single socket 
			// to accept both IPv4 and IPv6 connections
			// Otherwise, server needs to listen on two sockets,
			// one for IPv4 and one for IPv6
			// NOTE: dual-mode sockets are supported on Vista and later
			m_listener.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, 0);

			m_listener.Bind(new IPEndPoint(IPAddress.IPv6Any, 943));
			m_listener.Listen(backlog);
			_logger.Log("Policy server listening...");

			m_listener.BeginAccept(new AsyncCallback(OnConnection), null);
		}

		// Called when we receive a connection from a client
		public void OnConnection(IAsyncResult res)
		{
			Socket client = null;

			try
			{
				client = m_listener.EndAccept(res);
				_logger.Log("Policy server accepted connection.");
			}
			catch (SocketException)
			{
				return;
			}

			// handle this policy request with a PolicyConnection
			var pc = new PolicyConnection(client, m_policy, _logger);

			// look for more connections
			m_listener.BeginAccept(new AsyncCallback(OnConnection), null);
		}

		public void Close()
		{
			m_listener.Close();
		}
	}
}
