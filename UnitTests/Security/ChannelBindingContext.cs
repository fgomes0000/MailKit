﻿//
// ChannelBindingSource.cs
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

using System.Text;
using System.Security.Authentication.ExtendedProtection;

using MailKit.Net;

namespace UnitTests.Security {
	class ChannelBindingContext : IChannelBindingContext
	{
		readonly ChannelBindingKind supportedKind;
		readonly string data;

		public ChannelBindingContext (ChannelBindingKind kind, string channelBindingData)
		{
			data = channelBindingData;
			supportedKind = kind;
		}

		public bool TryGetChannelBinding (ChannelBindingKind kind, out ChannelBinding channelBinding)
		{
			channelBinding = null;
			return false;
		}

		public bool TryGetChannelBindingToken (ChannelBindingKind kind, out byte[] token)
		{
			if (kind == supportedKind) {
				token = Encoding.ASCII.GetBytes (data);
				return true;
			}

			token = null;
			return false;
		}
	}
}
