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
    #region Namespaces.

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Diagnostics;

    #endregion Namespaces.

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010", Justification = "required for this feature")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710", Justification = "required for this feature")]
    public class QueryOperationResponse : OperationResponse, System.Collections.IEnumerable
    {
        #region Private fields.

        private readonly DataServiceRequest query;

        private readonly MaterializeAtom results;

        #endregion Private fields.

        internal QueryOperationResponse(Dictionary<string, string> headers, DataServiceRequest query, MaterializeAtom results)
            : base(headers)
        {
            this.query = query;
            this.results = results;
        }

        public DataServiceRequest Query
        {
            get { return this.query; }
        }

        public virtual long TotalCount
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        internal MaterializeAtom Results
        {
            get
            {
                if (null != this.Error)
                {
                    throw Microsoft.Samples.Data.Services.Client.Error.InvalidOperation(Strings.Context_BatchExecuteError, this.Error);
                }

                return this.results;
            }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Results.GetEnumerator();
        }

        public DataServiceQueryContinuation GetContinuation()
        {
            return this.results.GetContinuation(null);
        }

        public DataServiceQueryContinuation GetContinuation(IEnumerable collection)
        {
            return this.results.GetContinuation(collection);
        }

        public DataServiceQueryContinuation<T> GetContinuation<T>(IEnumerable<T> collection)
        {
            return (DataServiceQueryContinuation<T>)this.results.GetContinuation(collection);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining | System.Runtime.CompilerServices.MethodImplOptions.NoOptimization)]
        internal static QueryOperationResponse GetInstance(Type elementType, Dictionary<string, string> headers, DataServiceRequest query, MaterializeAtom results)
        {
            Type genericType = typeof(QueryOperationResponse<>).MakeGenericType(elementType);
            System.Reflection.ConstructorInfo[] info = genericType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            System.Diagnostics.Debug.Assert(1 == info.Length, "only expected 1 ctor");

            return (QueryOperationResponse)Util.ConstructorInvoke(info[0], new object[] { headers, query, results });
        }
    }
}
