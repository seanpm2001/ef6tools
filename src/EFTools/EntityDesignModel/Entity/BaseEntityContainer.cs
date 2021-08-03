// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

namespace Microsoft.Data.Entity.Design.Model.Entity
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml.Linq;
    using Microsoft.Data.Entity.Design.Model.Mapping;

    internal abstract class BaseEntityContainer : NameableAnnotatableElement
    {
        internal static readonly string ElementName = "EntityContainer";
        private readonly List<EntitySet> _entitySets = new List<EntitySet>();
        private readonly List<AssociationSet> _associationSets = new List<AssociationSet>();

        protected BaseEntityContainer(EFElement parent, XElement element)
            : base(parent, element)
        {
        }

        internal void AddEntitySet(EntitySet set)
        {
            _entitySets.Add(set);
        }

        protected void ClearEntitySets()
        {
            ClearEFObjectCollection(_entitySets);
        }

        internal IEnumerable<EntitySet> EntitySets()
        {
            foreach (var es in _entitySets)
            {
                yield return es;
            }
        }

        internal int EntitySetCount
        {
            get { return _entitySets.Count; }
        }

        internal void AddAssociationSet(AssociationSet set)
        {
            _associationSets.Add(set);
        }

        internal IEnumerable<AssociationSet> AssociationSets()
        {
            foreach (var associationSet in _associationSets)
            {
                yield return associationSet;
            }
        }

        internal int AssociationSetCount
        {
            get { return _associationSets.Count; }
        }

        internal EntityContainerMapping EntityContainerMapping
        {
            get
            {
                foreach (var ecm in GetAntiDependenciesOfType<EntityContainerMapping>())
                {
                    return ecm;
                }
                return null;
            }
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
                foreach (var child1 in EntitySets())
                {
                    yield return child1;
                }

                foreach (var child2 in AssociationSets())
                {
                    yield return child2;
                }
            }
        }

        protected override void OnChildDeleted(EFContainer efContainer)
        {
            var child1 = efContainer as EntitySet;
            if (child1 != null)
            {
                _entitySets.Remove(child1);
                return;
            }

            var child2 = efContainer as AssociationSet;
            if (child2 != null)
            {
                _associationSets.Remove(child2);
                return;
            }

            base.OnChildDeleted(efContainer);
        }

#if DEBUG
        internal override ICollection<string> MyChildElementNames()
        {
            var s = base.MyChildElementNames();
            s.Add(EntitySet.ElementName);
            s.Add(AssociationSet.ElementName);
            return s;
        }
#endif

        protected override void PreParse()
        {
            Debug.Assert(State != EFElementState.Parsed, "this object should not already be in the parsed state");

            // clear _entitySets in child classes, as that is where this is populated
            ClearEFObjectCollection(_associationSets);
            base.PreParse();
        }

        internal override bool ParseSingleElement(ICollection<XName> unprocessedElements, XElement elem)
        {
            if (elem.Name.LocalName == AssociationSet.ElementName)
            {
                var assoc = new AssociationSet(this, elem);
                _associationSets.Add(assoc);
                assoc.Parse(unprocessedElements);
            }
            else
            {
                return base.ParseSingleElement(unprocessedElements, elem);
            }
            return true;
        }

        protected override void DoNormalize()
        {
            NormalizedName = new Symbol(LocalName.Value);
            base.DoNormalize();
        }

        internal override string EFTypeName
        {
            get { return ElementName; }
        }

        /// <summary>
        ///     EntityContainers are referred to just by their name, they are not scoped
        ///     by any schema namespaces
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        internal override string GetRefNameForBinding(ItemBinding binding)
        {
            return LocalName.Value;
        }

        public string EntityContainerName
        {
            get { return Name.Value; }
        }
    }
}
