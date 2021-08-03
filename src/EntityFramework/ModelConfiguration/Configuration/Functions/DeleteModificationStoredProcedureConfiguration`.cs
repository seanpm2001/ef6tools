// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

namespace System.Data.Entity.ModelConfiguration.Configuration
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Spatial;
    using System.Data.Entity.Utilities;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Allows configuration to be performed for a stored procedure that is used to delete entities.
    /// </summary>
    /// <typeparam name="TEntityType">The type of the entity that the stored procedure can be used to delete.</typeparam>
    public class DeleteModificationStoredProcedureConfiguration<TEntityType> : ModificationStoredProcedureConfigurationBase
        where TEntityType : class
    {
        internal DeleteModificationStoredProcedureConfiguration()
        {
        }

        /// <summary> Configures the name of the stored procedure. </summary>
        /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
        /// <param name="procedureName"> The stored procedure name. </param>
        public DeleteModificationStoredProcedureConfiguration<TEntityType> HasName(string procedureName)
        {
            Check.NotEmpty(procedureName, "procedureName");

            Configuration.HasName(procedureName);

            return this;
        }

        /// <summary>Configures the name of the stored procedure.</summary>
        /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
        /// <param name="procedureName">The stored procedure name.</param>
        /// <param name="schemaName">The schema name.</param>
        public DeleteModificationStoredProcedureConfiguration<TEntityType> HasName(string procedureName, string schemaName)
        {
            Check.NotEmpty(procedureName, "procedureName");
            Check.NotEmpty(schemaName, "schemaName");

            Configuration.HasName(procedureName, schemaName);

            return this;
        }

        /// <summary>Configures a parameter for this stored procedure.</summary>
        /// <typeparam name="TProperty">The type of the property to configure.</typeparam>
        /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
        /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <param name="parameterName">The name of the parameter.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public DeleteModificationStoredProcedureConfiguration<TEntityType> Parameter<TProperty>(
            Expression<Func<TEntityType, TProperty>> propertyExpression, string parameterName)
            where TProperty : struct
        {
            Check.NotNull(propertyExpression, "propertyExpression");
            Check.NotEmpty(parameterName, "parameterName");

            Configuration.Parameter(propertyExpression.GetComplexPropertyAccess(), parameterName);

            return this;
        }

        /// <summary>Configures a parameter for this stored procedure.</summary>
        /// <typeparam name="TProperty">The type of the property to configure.</typeparam>
        /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
        /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <param name="parameterName">The name of the parameter.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public DeleteModificationStoredProcedureConfiguration<TEntityType> Parameter<TProperty>(
            Expression<Func<TEntityType, TProperty?>> propertyExpression, string parameterName)
            where TProperty : struct
        {
            Check.NotNull(propertyExpression, "propertyExpression");
            Check.NotEmpty(parameterName, "parameterName");

            Configuration.Parameter(propertyExpression.GetComplexPropertyAccess(), parameterName);

            return this;
        }

        /// <summary>Configures a parameter for this stored procedure.</summary>
        /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
        /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <param name="parameterName">The name of the parameter.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public DeleteModificationStoredProcedureConfiguration<TEntityType> Parameter(
            Expression<Func<TEntityType, string>> propertyExpression, string parameterName)
        {
            Check.NotNull(propertyExpression, "propertyExpression");
            Check.NotEmpty(parameterName, "parameterName");

            Configuration.Parameter(propertyExpression.GetComplexPropertyAccess(), parameterName);

            return this;
        }

        /// <summary>Configures a parameter for this stored procedure.</summary>
        /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
        /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <param name="parameterName">The name of the parameter.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public DeleteModificationStoredProcedureConfiguration<TEntityType> Parameter(
            Expression<Func<TEntityType, byte[]>> propertyExpression, string parameterName)
        {
            Check.NotNull(propertyExpression, "propertyExpression");
            Check.NotEmpty(parameterName, "parameterName");

            Configuration.Parameter(propertyExpression.GetComplexPropertyAccess(), parameterName);

            return this;
        }

        /// <summary>Configures a parameter for this stored procedure.</summary>
        /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
        /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <param name="parameterName">The name of the parameter.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public DeleteModificationStoredProcedureConfiguration<TEntityType> Parameter(
            Expression<Func<TEntityType, DbGeography>> propertyExpression, string parameterName)
        {
            Check.NotNull(propertyExpression, "propertyExpression");
            Check.NotEmpty(parameterName, "parameterName");

            Configuration.Parameter(propertyExpression.GetComplexPropertyAccess(), parameterName);

            return this;
        }

        /// <summary>Configures a parameter for this stored procedure.</summary>
        /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
        /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <param name="parameterName">The name of the parameter.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public DeleteModificationStoredProcedureConfiguration<TEntityType> Parameter(
            Expression<Func<TEntityType, DbGeometry>> propertyExpression, string parameterName)
        {
            Check.NotNull(propertyExpression, "propertyExpression");
            Check.NotEmpty(parameterName, "parameterName");

            Configuration.Parameter(propertyExpression.GetComplexPropertyAccess(), parameterName);

            return this;
        }

        /// <summary>Configures the output parameter that returns the rows affected by this stored procedure.</summary>
        /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
        /// <param name="parameterName">The name of the parameter.</param>
        public DeleteModificationStoredProcedureConfiguration<TEntityType> RowsAffectedParameter(string parameterName)
        {
            Check.NotEmpty(parameterName, "parameterName");

            Configuration.RowsAffectedParameter(parameterName);

            return this;
        }

        /// <summary>Configures parameters for a relationship where the foreign key property is not included in the class.</summary>
        /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
        /// <param name="navigationPropertyExpression"> A lambda expression representing the navigation property for the relationship. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <param name="associationModificationStoredProcedureConfigurationAction">A lambda expression that performs the configuration.</param>
        /// <typeparam name="TPrincipalEntityType">The type of the principal entity in the relationship.</typeparam>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public DeleteModificationStoredProcedureConfiguration<TEntityType> Navigation<TPrincipalEntityType>(
            Expression<Func<TPrincipalEntityType, TEntityType>> navigationPropertyExpression,
            Action<AssociationModificationStoredProcedureConfiguration<TPrincipalEntityType>> associationModificationStoredProcedureConfigurationAction)
            where TPrincipalEntityType : class
        {
            Check.NotNull(navigationPropertyExpression, "navigationPropertyExpression");
            Check.NotNull(associationModificationStoredProcedureConfigurationAction, "associationModificationStoredProcedureConfigurationAction");

            var associationModificationStoredProcedureConfiguration
                = new AssociationModificationStoredProcedureConfiguration<TPrincipalEntityType>(
                    navigationPropertyExpression.GetSimplePropertyAccess().Single(),
                    Configuration);

            associationModificationStoredProcedureConfigurationAction(associationModificationStoredProcedureConfiguration);

            return this;
        }

        /// <summary>Configures parameters for a relationship where the foreign key property is not included in the class.</summary>
        /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
        /// <param name="navigationPropertyExpression"> A lambda expression representing the navigation property for the relationship. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <param name="associationModificationStoredProcedureConfigurationAction">A lambda expression that performs the configuration.</param>
        /// <typeparam name="TPrincipalEntityType">The type of the principal entity in the relationship.</typeparam>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public DeleteModificationStoredProcedureConfiguration<TEntityType> Navigation<TPrincipalEntityType>(
            Expression<Func<TPrincipalEntityType, ICollection<TEntityType>>> navigationPropertyExpression,
            Action<AssociationModificationStoredProcedureConfiguration<TPrincipalEntityType>> associationModificationStoredProcedureConfigurationAction)
            where TPrincipalEntityType : class
        {
            Check.NotNull(navigationPropertyExpression, "navigationPropertyExpression");
            Check.NotNull(associationModificationStoredProcedureConfigurationAction, "associationModificationStoredProcedureConfigurationAction");

            var associationModificationStoredProcedureConfiguration
                = new AssociationModificationStoredProcedureConfiguration<TPrincipalEntityType>(
                    navigationPropertyExpression.GetSimplePropertyAccess().Single(),
                    Configuration);

            associationModificationStoredProcedureConfigurationAction(associationModificationStoredProcedureConfiguration);

            return this;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }
    }
}
