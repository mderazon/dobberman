﻿// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

// Copyright 2010 Microsoft Corporation
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an 
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and limitations under the License.

namespace Microsoft.Samples.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.Samples.Data.Services.Http;

    public sealed class DataServiceStreamResponse : IDisposable
    {
        private HttpWebResponse response;

        private Dictionary<string, string> headers;

        internal DataServiceStreamResponse(HttpWebResponse response)
        {
            Debug.Assert(response != null, "Can't create a stream response object from a null response.");
            this.response = response;
        }

        public string ContentType
        {
            get
            {
                this.CheckDisposed();
                return this.response.Headers[XmlConstants.HttpContentType];
            }
        }

        public string ContentDisposition
        {
            get
            {
                this.CheckDisposed();
                return this.response.Headers[XmlConstants.HttpContentDisposition];
            }
        }

        public Dictionary<string, string> Headers
        {
            get
            {
                this.CheckDisposed();
                if (this.headers == null)
                {
                    this.headers = WebUtil.WrapResponseHeaders(this.response);
                }

                return this.headers;
            }
        }

        public Stream Stream
        {
            get
            {
                this.CheckDisposed();
                return this.response.GetResponseStream();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Util.Dispose(ref this.response);
        }

        #endregion

        private void CheckDisposed()
        {
            if (this.response == null)
            {
                Error.ThrowObjectDisposed(this.GetType());
            }
        }
    }
}
