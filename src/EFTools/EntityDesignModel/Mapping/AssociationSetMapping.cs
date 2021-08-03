﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

namespace Microsoft.Data.Entity.Design.Model.Mapping
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Xml.Linq;
    using Microsoft.Data.Entity.Design.Model.Entity;

    internal class AssociationSetMapping : EFElement
    {
        internal static readonly string ElementName = "AssociationSetMapping";
        internal static readonly string AttributeName = "Name";
        internal static readonly string AttributeTypeName = "TypeName";
        internal static readonly string AttributeStoreEntitySet = "StoreEntitySet";

        private readonly List<EndProperty> _endProperties = new List<EndProperty>();
        private readonly List<Condition> _conditions = new List<Condition>();
        private QueryView _queryView;
        private SingleItemBinding<AssociationSet> _name;
        private SingleItemBinding<Association> _typeName;
        private SingleItemBinding<EntitySet> _tableName;

        internal AssociationSetMapping(EFElement parent, XElement element)
            : base(parent, element)
        {
        }

        /// <summary>
        ///     A bindable reference to the AssociationSet in the C-space model
        /// </summary>
        internal SingleItemBinding<AssociationSet> Name
        {
            get
            {
                if (_name == null)
                {
                    _name = new SingleItemBinding<AssociationSet>(
                        this,
                        AttributeName,
                        AssociationSetNameNormalizer.NameNormalizer);
                }
                return _name;
            }
        }

        /// <summary>
        ///     A bindable reference to the Association in the referenced AssociationSet
        /// </summary>
        internal SingleItemBinding<Association> TypeName
        {
            get
            {
                if (_typeName == null)
                {
                    _typeName = new SingleItemBinding<Association>(
                        this,
                        AttributeTypeName,
                        AssociationNameNormalizer.NameNormalizer);
                }
                return _typeName;
            }
        }

        /// <summary>
        ///     A bindable reference to the EntitySet in the S-space model
        /// </summary>
        internal SingleItemBinding<EntitySet> StoreEntitySet
        {
            get
            {
                if (_tableName == null)
                {
                    _tableName = new SingleItemBinding<EntitySet>(
                        this,
                        AttributeStoreEntitySet,
                        EntitySetNameNormalizer.NameNormalizer);
                }

                return _tableName;
            }
        }

        internal void AddEndProperty(EndProperty ep)
        {
            _endProperties.Add(ep);
        }

        internal IList<EndProperty> EndProperties()
        {
            return _endProperties.AsReadOnly();
        }

        internal void AddCondition(Condition cond)
        {
            _conditions.Add(cond);
        }

        internal IList<Condition> Conditions()
        {
            return _conditions.AsReadOnly();
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

                foreach (var child in EndProperties())
                {
                    yield return child;
                }

                foreach (var child in Conditions())
                {
                    yield return child;
                }

                yield return Name;
                yield return TypeName;
                yield return StoreEntitySet;

                if (QueryView != null)
                {
                    yield return QueryView;
                }
            }
        }

        internal QueryView QueryView
        {
            get { return _queryView; }
        }

        internal bool HasQueryViewElement
        {
            get { return (null != QueryView); }
        }

        protected override void OnChildDeleted(EFContainer efContainer)
        {
            var child1 = efContainer as EndProperty;
            if (child1 != null)
            {
                _endProperties.Remove(child1);
                return;
            }

            var child2 = efContainer as Condition;
            if (child2 != null)
            {
                _conditions.Remove(child2);
                return;
            }

            base.OnChildDeleted(efContainer);
        }

#if DEBUG
        internal override ICollection<string> MyAttributeNames()
        {
            var s = base.MyAttributeNames();
            s.Add(AttributeName);
            s.Add(AttributeTypeName);
            s.Add(AttributeStoreEntitySet);
            return s;
        }
#endif

#if DEBUG
        internal override ICollection<string> MyChildElementNames()
        {
            var s = base.MyChildElementNames();
            s.Add(Condition.ElementName);
            s.Add(EndProperty.ElementName);
            s.Add(QueryView.ElementName);
            return s;
        }
#endif

        protected override void PreParse()
        {
            Debug.Assert(State != EFElementState.Parsed, "this object should not already be in the parsed state");

            ClearEFObject(_name);
            _name = null;
            ClearEFObject(_typeName);
            _typeName = null;
            ClearEFObject(_tableName);
            _tableName = null;
            ClearEFObject(_queryView);
            _queryView = null;

            ClearEFObjectCollection(_endProperties);
            ClearEFObjectCollection(_conditions);

            base.PreParse();
        }

        private string DisplayNameInternal(bool localize)
        {
            string resource;
            if (localize)
            {
                resource = Resources.MappingModel_AssociationSetMappingDisplayName;
            }
            else
            {
                resource = "{0} (AssociationSet)";
            }
            return string.Format(
                CultureInfo.CurrentCulture,
                resource,
                Name.RefName);
        }

        internal override string DisplayName
        {
            get { return DisplayNameInternal(true); }
        }

        internal override string NonLocalizedDisplayName
        {
            get { return DisplayNameInternal(false); }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        internal override bool ParseSingleElement(ICollection<XName> unprocessedElements, XElement elem)
        {
            if (elem.Name.LocalName == EndProperty.ElementName)
            {
                var ep = new EndProperty(this, elem);
                ep.Parse(unprocessedElements);
                _endProperties.Add(ep);
            }
            else if (elem.Name.LocalName == Condition.ElementName)
            {
                var c = new Condition(this, elem);
                c.Parse(unprocessedElements);
                _conditions.Add(c);
            }
            else if (elem.Name.LocalName == QueryView.ElementName)
            {
                Debug.Assert(
                    _queryView == null, "There could only be 1 instance of QueryView element inside AssociationSetMapping element.");
                _queryView = new QueryView(this, elem);
                _queryView.Parse(unprocessedElements);
            }
            else
            {
                return base.ParseSingleElement(unprocessedElements, elem);
            }
            return true;
        }

        protected override void DoResolve(EFArtifactSet artifactSet)
        {
            Name.Rebind();
            TypeName.Rebind();
            StoreEntitySet.Rebind();
            if (Name.Status == BindingStatus.Known
                && TypeName.Status == BindingStatus.Known
                && StoreEntitySet.Status == BindingStatus.Known)
            {
                State = EFElementState.Resolved;
            }
        }

        internal override void GetXLinqInsertPosition(EFElement child, out XNode insertAt, out bool insertBefore)
        {
            if (child is EndProperty)
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
