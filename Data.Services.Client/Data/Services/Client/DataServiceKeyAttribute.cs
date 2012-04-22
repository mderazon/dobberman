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

namespace Microsoft.Samples.Data.Services.Common
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Microsoft.Samples.Data.Services.Client;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Accessors are available for processed input.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DataServiceKeyAttribute : System.Attribute
    {
        private readonly ReadOnlyCollection<string> keyNames;

        public DataServiceKeyAttribute(string keyName)
        {
            Util.CheckArgumentNull(keyName, "keyName");
            Util.CheckArgumentNotEmpty(keyName, "KeyName");
            this.keyNames = new ReadOnlyCollection<string>(new string[1] { keyName });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "parameters are validated against null via CheckArgumentNull")]
        public DataServiceKeyAttribute(params string[] keyNames)
        {
            Util.CheckArgumentNull(keyNames, "keyNames");
            if (keyNames.Length == 0 || keyNames.Any(f => f == null || f.Length == 0))
            {
                throw Error.Argument(Strings.DSKAttribute_MustSpecifyAtleastOnePropertyName, "keyNames");
            }

            this.keyNames = new ReadOnlyCollection<string>(keyNames);
        }

        public ReadOnlyCollection<string> KeyNames
        {
            get
            {
                return this.keyNames;
            }
        }
    }
}
