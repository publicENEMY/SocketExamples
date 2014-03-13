SocketExamples
==============

Network programming example(echo server and client) that uses SocketAsyncEventArgs, 
c# and GUI for Wpf and Silverlight.

Based on MDSN Example 
1.	Socket Performance Technology Sample. http://archive.msdn.microsoft.com/nclsamples/Wiki/View.aspx?title=Socket%20Performance
2.	Sample Code for a Policy Server for Sockets. http://msdn.microsoft.com/en-us/library/cc645032(v=vs.95).aspx

Overview
========

This is a basic example of socket programming for .Net. This example contains
1.	WpfServer
	Contains a socket server which echo what client sent back to the client 
	and a policy server which are required for Silverlight application.
2.  WpfClient
	A socket client that sends message to the server and wait for response(echo) 
	from the server. In Wpf.
3.	SilverlightClient
	A socket client that sends message to the server and wait for response(echo)
	from the server. In Silverlight.
4.	SilverlightClient.Web
	A website that host SilverlightClient.
	
This example also contains extras not featured in MSDN. Noteable extras
1.	Logging system.
2.	GUI based application. MSDN example is a console application.

Motivation
==========

Developing network application thats able to handle large number of clients 
efficiently was very hard. .Net 3.5 introduces SocketAsyncEventArgs which simplify 
this task. For more information, see Socket Performance Enhancements in Version 3.5. 
http://msdn.microsoft.com/en-us/library/bb968780(v=vs.110).aspx. 

This application aims to provide a simple gui based echo server application 
that uses System.Net.Sockets.SocketAsyncEventArgs in Wpf and Silverlight.

How to use
==========

1.	Compile and build all in Visual Studio 2013.
2.	Run WpfServer, WpfClient SilverlightClient(independently or through SilverlightClient.Web).
3.	WpfServer will start policy server (for Silverlight) and echo server 
	(for Silverlight and Wpf)
4.	Click Connect and Send button in WpfClient or SilverlightClient. 