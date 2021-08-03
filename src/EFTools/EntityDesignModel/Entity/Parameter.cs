﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

namespace Microsoft.Data.Entity.Design.Model.Entity
{
    using System.Collections.Generic;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml.Linq;

    /// <summary>
    ///     This XML element is used in both the C and S Sides.  In a conceptual model, this will
    ///     be a child of a FunctionImport element.  In the storage model, this will be a child
    ///     of a Function element.
    /// </summary>
    internal class Parameter : NameableAnnotatableElement
    {
        internal enum InOutMode
        {
            Unknown,
            In,
            Out,
            InOut
        }

        private static readonly HashSet<PrimitiveTypeKind> RowsAffectedParameterCompatibleTypes = new HashSet<PrimitiveTypeKind>();

        internal static readonly string ElementName = "Parameter";
        internal static readonly string AttributeType = "Type";
        internal static readonly string AttributeMode = "Mode";

        internal const string ModeIn = "In";
        internal const string ModeOut = "Out";
        internal const string ModeInOut = "InOut";

        private DefaultableValue<string> _typeAttr;
        private DefaultableValue<string> _modeAttr;

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static Parameter()
        {
            RowsAffectedParameterCompatibleTypes.Add(PrimitiveTypeKind.Byte);
            RowsAffectedParameterCompatibleTypes.Add(PrimitiveTypeKind.Int16);
            RowsAffectedParameterCompatibleTypes.Add(PrimitiveTypeKind.Int32);
            RowsAffectedParameterCompatibleTypes.Add(PrimitiveTypeKind.Int64);
            RowsAffectedParameterCompatibleTypes.Add(PrimitiveTypeKind.SByte);
        }

        internal Parameter(EFElement parent, XElement element)
            : base(parent, element)
        {
            Debug.Assert(parent is FunctionImport || parent is Function, "parent should be a FunctionImport or a Function");
        }

        internal DefaultableValue<string> Type
        {
            get
            {
                if (_typeAttr == null)
                {
                    _typeAttr = new TypeDefaultableValue(this);
                }
                return _typeAttr;
            }
        }

        private class TypeDefaultableValue : DefaultableValue<string>
        {
            internal TypeDefaultableValue(EFElement parent)
                : base(parent, AttributeType)
            {
            }

            internal override string AttributeName
            {
                get { return AttributeType; }
            }

            public override string DefaultValue
            {
                get { return null; }
            }
        }

        internal DefaultableValue<string> Mode
        {
            get
            {
                if (_modeAttr == null)
                {
                    _modeAttr = new ModeDefaultableValue(this);
                }
                return _modeAttr;
            }
        }

        private class ModeDefaultableValue : DefaultableValue<string>
        {
            internal ModeDefaultableValue(EFElement parent)
                : base(parent, AttributeMode)
            {
            }

            internal override string AttributeName
            {
                get { return AttributeMode; }
            }

            public override string DefaultValue
            {
                get { return null; }
            }
        }

        internal InOutMode InOut
        {
            get
            {
                switch (Mode.Value)
                {
                    case ModeIn:
                        return InOutMode.In;
                    case ModeOut:
                        return InOutMode.Out;
                    case ModeInOut:
                        return InOutMode.InOut;
                    default:
                        return InOutMode.Unknown;
                }
            }
        }

        protected override void PreParse()
        {
            Debug.Assert(State != EFElementState.Parsed, "this object should not already be in the parsed state");

            ClearEFObject(_typeAttr);
            _typeAttr = null;

            ClearEFObject(_modeAttr);
            _modeAttr = null;

            base.PreParse();
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
                yield return Type;
                yield return Mode;
            }
        }

        protected override void DoNormalize()
        {
            var normalizedName = ParameterNameNormalizer.NameNormalizer(Parent as EFElement, LocalName.Value);
            Debug.Assert(null != normalizedName, "Null NormalizedName for refName " + LocalName.Value);
            NormalizedName = (normalizedName != null ? normalizedName.Symbol : Symbol.EmptySymbol);
            base.DoNormalize();
        }

        /// <summary>
        ///     A parameter is always referred to by its bare Name.  This is because a parameter is always
        ///     referred to in the context of a Function.
        /// </summary>
        internal override string GetRefNameForBinding(ItemBinding binding)
        {
            return LocalName.Value;
        }

        internal bool CanBeUsedAsRowsAffectedParameter()
        {
            // must be on Storage-side of model
            var f = Parent as Function;
            if (null == f)
            {
                return false;
            }
            var sem = f.EntityModel;
            if (null == sem)
            {
                return false;
            }

            // InOutMode must be 'Out' or 'InOut'
            if (InOutMode.Out != InOut
                && InOutMode.InOut != InOut)
            {
                return false;
            }

            // Parameter Type must be compatible with being a "number of rows affected"
            var type = Type.Value;
            if (null == type)
            {
                return false;
            }
            var storagePrimType = sem.GetStoragePrimitiveType(type);
            if (null == storagePrimType)
            {
                return false;
            }
            return RowsAffectedParameterCompatibleTypes.Contains(storagePrimType.PrimitiveTypeKind);
        }
    }
}
