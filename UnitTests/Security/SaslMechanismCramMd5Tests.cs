﻿//
// SaslMechanismCramMd5Tests.cs
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

using System.Net;
using System.Text;

using MailKit.Security;

namespace UnitTests.Security {
	[TestFixture]
	public class SaslMechanismCramMd5Tests
	{
		[Test]
		public void TestArgumentExceptions ()
		{
			var credentials = new NetworkCredential ("username", "password");

			var sasl = new SaslMechanismCramMd5 (credentials);
			Assert.Throws<NotSupportedException> (() => sasl.Challenge (null));

			Assert.Throws<ArgumentNullException> (() => new SaslMechanismCramMd5 (null));
			Assert.Throws<ArgumentNullException> (() => new SaslMechanismCramMd5 (null, "password"));
			Assert.Throws<ArgumentNullException> (() => new SaslMechanismCramMd5 ("username", null));
		}

		static void AssertExampleFromRfc2195 (SaslMechanismCramMd5 sasl, string prefix)
		{
			const string serverToken = "<1896.697170952@postoffice.example.net>";
			const string expected = "joe 3dbc88f0624776a737b39093f6eb6427";

			Assert.That (sasl.SupportsChannelBinding, Is.False, $"{prefix}: SupportsChannelBinding");
			Assert.That (sasl.SupportsInitialResponse, Is.False, $"{prefix}: SupportsInitialResponse");

			var token = Encoding.ASCII.GetBytes (serverToken);
			var challenge = sasl.Challenge (Convert.ToBase64String (token));
			var decoded = Convert.FromBase64String (challenge);
			var result = Encoding.ASCII.GetString (decoded);

			Assert.That (result, Is.EqualTo (expected), $"{prefix}: challenge response does not match the expected string.");
			Assert.That (sasl.IsAuthenticated, Is.True, $"{prefix}: should be authenticated now.");
			Assert.That (sasl.NegotiatedChannelBinding, Is.False, $"{prefix}: NegotiatedChannelBinding");
			Assert.That (sasl.NegotiatedSecurityLayer, Is.False, $"{prefix}: NegotiatedSecurityLayer");

			Assert.That (sasl.Challenge (string.Empty), Is.EqualTo (string.Empty), $"{prefix}: challenge while authenticated.");
		}

		[Test]
		public void TestExampleFromRfc2195 ()
		{
			var credentials = new NetworkCredential ("joe", "tanstaaftanstaaf");
			var sasl = new SaslMechanismCramMd5 (credentials);
			var uri = new Uri ("smtp://elwood.innosoft.com");

			AssertExampleFromRfc2195 (sasl, "NetworkCredential");

			sasl = new SaslMechanismCramMd5 ("joe", "tanstaaftanstaaf");

			AssertExampleFromRfc2195 (sasl, "user/pass");
		}
	}
}
