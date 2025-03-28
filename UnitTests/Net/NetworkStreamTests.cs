﻿//
// NetworkStreamTests.cs
//
// Author: Jeffrey Stedfast <jestedfa@microsoft.com>
//
// Copyright (c) 2013-2025 .NET Foundation and Contributors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System.Net.Sockets;

using NetworkStream = MailKit.Net.NetworkStream;

namespace UnitTests.Net {
	[TestFixture]
	public class NetworkStreamTests : IDisposable
	{
		readonly Socket socket;

		public NetworkStreamTests ()
		{
			socket = new Socket (SocketType.Stream, ProtocolType.Tcp);
			socket.Connect ("www.google.com", 80);
		}

		public void Dispose ()
		{
			socket.Dispose ();
			GC.SuppressFinalize (this);
		}

		[Test]
		public void TestCanReadWriteSeekTimeout ()
		{
			using (var stream = new NetworkStream (socket, false)) {
				Assert.That (stream.CanRead, Is.True, "CanRead");
				Assert.That (stream.CanWrite, Is.True, "CanWrite");
				Assert.That (stream.CanSeek, Is.False, "CanSeek");
				Assert.That (stream.CanTimeout, Is.True, "CanTimeout");
			}
		}

		[Test]
		public void TestNotSupportedExceptions ()
		{
			using (var stream = new NetworkStream (socket, false)) {
				Assert.Throws<NotSupportedException> (() => { var x = stream.Length; });
				Assert.Throws<NotSupportedException> (() => stream.SetLength (512));
				Assert.Throws<NotSupportedException> (() => { var x = stream.Position; });
				Assert.Throws<NotSupportedException> (() => { stream.Position = 512; });
				Assert.Throws<NotSupportedException> (() => stream.Seek (512, SeekOrigin.Begin));
			}
		}

		[Test]
		public void TestTimeouts ()
		{
			using (var stream = new NetworkStream (socket, false)) {
				Assert.That (stream.ReadTimeout, Is.EqualTo (Timeout.Infinite), "ReadTimeout #1");
				Assert.Throws<ArgumentOutOfRangeException> (() => stream.ReadTimeout = 0);
				Assert.Throws<ArgumentOutOfRangeException> (() => stream.ReadTimeout = -2);
				stream.ReadTimeout = 500;
				Assert.That (stream.ReadTimeout, Is.EqualTo (500), "ReadTimeout #2");
				stream.ReadTimeout = Timeout.Infinite;
				Assert.That (stream.ReadTimeout, Is.EqualTo (Timeout.Infinite), "ReadTimeout #3");

				Assert.That (stream.WriteTimeout, Is.EqualTo (Timeout.Infinite), "WriteTimeout #1");
				Assert.Throws<ArgumentOutOfRangeException> (() => stream.WriteTimeout = 0);
				Assert.Throws<ArgumentOutOfRangeException> (() => stream.WriteTimeout = -2);
				stream.WriteTimeout = 500;
				Assert.That (stream.WriteTimeout, Is.EqualTo (500), "WriteTimeout #2");
				stream.WriteTimeout = Timeout.Infinite;
				Assert.That (stream.WriteTimeout, Is.EqualTo (Timeout.Infinite), "WriteTimeout #3");
			}
		}
	}
}
