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

    using System.Collections.Generic;
    using System.Diagnostics;

    #endregion Namespaces.

    [DebuggerDisplay("AtomContentProperty {TypeName} {Name}")]
    internal class AtomContentProperty
    {
        public bool IsNull
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string TypeName
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public List<AtomContentProperty> Properties
        {
            get;
            set;
        }

        public AtomFeed Feed
        {
            get;
            set;
        }

        public AtomEntry Entry
        {
            get;
            set;
        }

        public object MaterializedValue
        {
            get;
            set;
        }

        public void EnsureProperties()
        {
            if (this.Properties == null)
            {
                this.Properties = new List<AtomContentProperty>();
            }
        }
    }
}
