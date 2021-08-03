﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

namespace Microsoft.Data.Entity.Design.Model.Mapping
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml.Linq;

    internal class ResultMapping : EFElement
    {
        internal static readonly string ElementName = "ResultMapping";

        private readonly List<FunctionImportTypeMapping> _typeMappings = new List<FunctionImportTypeMapping>();

        internal ResultMapping(FunctionImportMapping parent, XElement element)
            : base(parent, element)
        {
        }

        internal IList<FunctionImportTypeMapping> TypeMappings()
        {
            return _typeMappings.AsReadOnly();
        }

        internal void AddTypeMapping(FunctionImportTypeMapping typeMapping)
        {
            _typeMappings.Add(typeMapping);
        }

        internal FunctionImportTypeMapping FindTypeMapping(EFNormalizableItem type)
        {
            foreach (var typeMapping in _typeMappings)
            {
                if (typeMapping.TypeName.Target == type)
                {
                    return typeMapping;
                }
            }
            return null;
        }

#if DEBUG
        internal override ICollection<string> MyChildElementNames()
        {
            var s = base.MyChildElementNames();
            s.Add(FunctionImportEntityTypeMapping.ElementName);
            s.Add(FunctionImportComplexTypeMapping.ElementName);
            return s;
        }
#endif

        protected override void PreParse()
        {
            Debug.Assert(State != EFElementState.Parsed, "this object should not already be in the parsed state");

            ClearEFObjectCollection(_typeMappings);

            base.PreParse();
        }

        internal override bool ParseSingleElement(ICollection<XName> unprocessedElements, XElement elem)
        {
            if (elem.Name.LocalName == FunctionImportEntityTypeMapping.ElementName)
            {
                var etm = new FunctionImportEntityTypeMapping(this, elem);
                _typeMappings.Add(etm);
                etm.Parse(unprocessedElements);
            }
            else if (elem.Name.LocalName == FunctionImportComplexTypeMapping.ElementName)
            {
                var ctm = new FunctionImportComplexTypeMapping(this, elem);
                _typeMappings.Add(ctm);
                ctm.Parse(unprocessedElements);
            }
            else
            {
                return base.ParseSingleElement(unprocessedElements, elem);
            }
            return true;
        }

        // we unfortunately get a warning from the compiler when we use the "base" keyword in "iterator" types generated by using the
        // "yield return" keyword.  By adding this method, I was able to get around this.  Unfortunately, I wasn't able to figure out
        // a way to implement this once and have derived classes share the implementation (since the "base" keyword is resolved at 
        // compile-time and not at runtime.
        private IEnumerable<EFObject> BaseChildren
        {
            get { return base.Children; }
        }

        internal override IEnumerable<EFObject> Children
        {
            get
            {
                foreach (var efobj in BaseChildren)
                {
                    yield return efobj;
                }

                foreach (var fitm in _typeMappings)
                {
                    yield return fitm;
                }
            }
        }

        protected override void OnChildDeleted(EFContainer efContainer)
        {
            var fitm = efContainer as FunctionImportTypeMapping;
            if (fitm != null)
            {
                _typeMappings.Remove(fitm);
                return;
            }

            base.OnChildDeleted(efContainer);
        }
    }
}
