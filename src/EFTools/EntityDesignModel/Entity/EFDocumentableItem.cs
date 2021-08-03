// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

namespace Microsoft.Data.Entity.Design.Model.Entity
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Microsoft.Data.Entity.Design.Common;

    internal abstract class EFDocumentableItem : EFNameableItem
    {
        private Documentation _documentation;

        internal EFDocumentableItem(EFElement parent, XElement element)
            : base(parent, element)
        {
        }

        internal override bool HasDocumentationElement
        {
            get { return true; }
        }

        internal override EFContainer DocumentationEFContainer
        {
            get { return Documentation; }
        }

        internal Documentation Documentation
        {
            get { return _documentation; }
            set { _documentation = value; }
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

                if (Documentation != null)
                {
                    yield return Documentation;
                }
            }
        }

#if DEBUG
        // only used in DEBUG to identify xml elements that we aren't processing
        internal override ICollection<string> MyChildElementNames()
        {
            var s = base.MyChildElementNames();
            s.Add(Documentation.ElementName);
            return s;
        }
#endif

        protected override void OnChildDeleted(EFContainer efContainer)
        {
            if (efContainer is Documentation)
            {
                _documentation = null;
            }

            base.OnChildDeleted(efContainer);
        }

        protected override void PreParse()
        {
            ClearEFObject(_documentation);
            _documentation = null;

            base.PreParse();
        }

        internal override bool ParseSingleElement(ICollection<XName> unprocessedElements, XElement element)
        {
            if (element.Name.LocalName == Documentation.ElementName)
            {
                if (_documentation != null)
                {
                    // multiple Documentation elements
                    Artifact.AddParseErrorForObject(
                        this, Resources.TOO_MANY_DOCUMENTATION_ELEMENTS, ErrorCodes.TOO_MANY_DOCUMENTATION_ELEMENTS);
                }
                else
                {
                    _documentation = new Documentation(this, element);
                    _documentation.Parse(unprocessedElements);
                }

                return true;
            }

            return base.ParseSingleElement(unprocessedElements, element);
        }

        internal override void GetXLinqInsertPosition(EFElement child, out XNode insertAt, out bool insertBefore)
        {
            if (child is Documentation)
            {
                insertAt = FirstChildXElementOrNull();
                insertBefore = true;
            }
            else
            {
                base.GetXLinqInsertPosition(child, out insertAt, out insertBefore);
            }
        }
    }
}
