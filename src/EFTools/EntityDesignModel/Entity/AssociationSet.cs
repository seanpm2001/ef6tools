// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

namespace Microsoft.Data.Entity.Design.Model.Entity
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml.Linq;
    using Microsoft.Data.Entity.Design.Model.Mapping;

    internal class AssociationSet : NameableAnnotatableElement
    {
        internal static readonly string ElementName = "AssociationSet";
        internal static readonly string AttributeAssociation = "Association";

        private SingleItemBinding<Association> _associationBinding;
        private readonly List<AssociationSetEnd> _ends = new List<AssociationSetEnd>();

        internal AssociationSet(EFElement parent, XElement element)
            : base(parent, element)
        {
        }

        internal BaseEntityModel EntityModel
        {
            get { return this.RuntimeModelRoot() as BaseEntityModel; }
        }

        /// <summary>
        ///     A bindable reference to the Association included in this set
        /// </summary>
        internal SingleItemBinding<Association> Association
        {
            get
            {
                if (_associationBinding == null)
                {
                    _associationBinding = new SingleItemBinding<Association>(
                        this,
                        AttributeAssociation,
                        AssociationNameNormalizer.NameNormalizer);
                }
                return _associationBinding;
            }
        }

        internal void AddAssociationSetEnd(AssociationSetEnd end)
        {
            _ends.Add(end);
        }

        internal IList<AssociationSetEnd> AssociationSetEnds()
        {
            return _ends.AsReadOnly();
        }

        internal AssociationSetEnd PrincipalEnd
        {
            get
            {
                var association = Association.Target;
                if (association != null
                    && association.ReferentialConstraint != null
                    && association.ReferentialConstraint.Principal != null)
                {
                    foreach (var associationSetEnd in _ends)
                    {
                        if (associationSetEnd.Role.Target ==
                            association.ReferentialConstraint.Principal.Role.Target)
                        {
                            return associationSetEnd;
                        }
                    }
                }
                return null;
            }
        }

        internal AssociationSetEnd DependentEnd
        {
            get
            {
                var association = Association.Target;
                if (association != null
                    && association.ReferentialConstraint != null
                    && association.ReferentialConstraint.Dependent != null)
                {
                    foreach (var associationSetEnd in _ends)
                    {
                        if (associationSetEnd.Role.Target ==
                            association.ReferentialConstraint.Dependent.Role.Target)
                        {
                            return associationSetEnd;
                        }
                    }
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
                foreach (var child in AssociationSetEnds())
                {
                    yield return child;
                }
                yield return Association;
            }
        }

        protected override void OnChildDeleted(EFContainer efContainer)
        {
            var child = efContainer as AssociationSetEnd;
            if (child != null)
            {
                _ends.Remove(child);
                return;
            }

            base.OnChildDeleted(efContainer);
        }

#if DEBUG
        internal override ICollection<string> MyAttributeNames()
        {
            var s = base.MyAttributeNames();
            s.Add(AttributeAssociation);
            return s;
        }
#endif

#if DEBUG
        internal override ICollection<string> MyChildElementNames()
        {
            var s = base.MyChildElementNames();
            s.Add(AssociationSetEnd.ElementName);
            return s;
        }
#endif

        protected override void PreParse()
        {
            Debug.Assert(State != EFElementState.Parsed, "this object should not already be in the parsed state");

            ClearEFObject(_associationBinding);
            _associationBinding = null;
            ClearEFObjectCollection(_ends);

            _ends.Clear();

            base.PreParse();
        }

        internal override bool ParseSingleElement(ICollection<XName> unprocessedElements, XElement elem)
        {
            if (elem.Name.LocalName == AssociationSetEnd.ElementName)
            {
                var ase = new AssociationSetEnd(this, elem);
                _ends.Add(ase);
                ase.Parse(unprocessedElements);
            }
            else
            {
                return base.ParseSingleElement(unprocessedElements, elem);
            }
            return true;
        }

        protected override void DoNormalize()
        {
            var normalizedName = AssociationSetNameNormalizer.NameNormalizer(this, LocalName.Value);
            Debug.Assert(null != normalizedName, "Null for refName " + LocalName.Value);
            NormalizedName = (normalizedName != null ? normalizedName.Symbol : Symbol.EmptySymbol);
            base.DoNormalize();
        }

        protected override void DoResolve(EFArtifactSet artifactSet)
        {
            Association.Rebind();
            if (Association.Status == BindingStatus.Known)
            {
                State = EFElementState.Resolved;
            }
        }

        internal ICollection<AssociationSetMapping> AssociationSetMappings
        {
            get
            {
                var antiDeps = Artifact.ArtifactSet.GetAntiDependencies(this);

                var mappings = new List<AssociationSetMapping>();
                foreach (var antiDep in antiDeps)
                {
                    var asm = antiDep as AssociationSetMapping;
                    if (asm == null
                        && antiDep.Parent != null)
                    {
                        asm = antiDep.Parent as AssociationSetMapping;
                    }

                    if (asm != null)
                    {
                        mappings.Add(asm);
                    }
                }

                return mappings.AsReadOnly();
            }
        }

        internal AssociationSetMapping AssociationSetMapping
        {
            get
            {
                foreach (var asm in AssociationSetMappings)
                {
                    // just return the first one
                    return asm;
                }

                return null;
            }
        }

        /// <summary>
        ///     An AssociationSet is always referred to by its bare Name.  This is because an AssociationSet is always
        ///     referred to in the context of an EntityContainer.
        /// </summary>
        internal override string GetRefNameForBinding(ItemBinding binding)
        {
            return LocalName.Value;
        }
    }
}
