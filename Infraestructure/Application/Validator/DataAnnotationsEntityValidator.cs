﻿using Application.IValidator;
using Domain.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Application.Validator
{
    /// <summary>
    ///   Validator based on Data Annotations. 
    ///   This validator use IValidatableObject interface and
    ///   ValidationAttribute ( hierachy of this) for
    ///   perform validation. 
    ///   This is not a DataAnnotation
    /// </summary>
    public class DataAnnotationsEntityValidator : IEntityValidator
    {
        #region Private Methods

        /// <summary>
        ///   Get erros if object implement IValidatableObject
        /// </summary>
        /// <typeparam name="TEntity"> The typeof entity </typeparam>
        /// <param name="item"> The item to validate </param>
        /// <param name="errors"> A collection of current errors </param>
        private static void SetValidatableObjectErrors<TEntity>(TEntity item, List<string> errors) where TEntity : class
        {
            if (typeof(IValidatableObject).IsAssignableFrom(typeof(TEntity)))
            {
                var validationContext = new ValidationContext(item, null, null);

                var validationResults = ((IValidatableObject)item).Validate(validationContext);

                
                errors.AddRange(validationResults.Select(vr => vr.ErrorMessage+""));
            }
        }

        /// <summary>
        ///   Get errors on ValidationAttribute
        /// </summary>
        /// <typeparam name="TEntity"> The type of entity </typeparam>
        /// <param name="item"> The entity to validate </param>
        /// <param name="errors"> A collection of current errors </param>
        private static void SetValidationAttributeErrors<TEntity>(TEntity item, List<string> errors) where TEntity : class
        {
            //TODO: Capture attributes that are decorated in metadata class
            var result = (from property in TypeDescriptor.GetProperties(item).Cast<PropertyDescriptor>()
                          from attribute in property.Attributes.OfType<ValidationAttribute>()
                          where !attribute.IsValid(property.GetValue(item))
                          select attribute.FormatErrorMessage(string.Empty)).ToList();
            if (result.Any())
                errors.AddRange(result);
        }

        #endregion

        #region IEntityValidator Members

        /// <summary>
        ///   <see cref="IEntityValidator" />
        /// </summary>
        /// <typeparam name="TEntity"> <see
        ///    cref="IEntityValidator" /> </typeparam>
        /// <param name="item"> <see cref="IEntityValidator" /> </param>
        /// <returns> <see cref="IEntityValidator" /> </returns>
        public bool IsValid<TEntity>(TEntity item) where TEntity : class
        {
            if (item == null)
                return false;

            var validationErrors = new List<string>();

            SetValidatableObjectErrors(item, validationErrors);
            SetValidationAttributeErrors(item, validationErrors);

            return !validationErrors.Any();
        }

        /// <summary>
        ///   <see cref="IEntityValidator" />
        /// </summary>
        /// <typeparam name="TEntity"> <see
        ///    cref="IEntityValidator" /> </typeparam>
        /// <param name="item"> <see cref="IEntityValidator" /> </param>
        /// <returns> <see cref="IEntityValidator" /> </returns>
        public List<string> GetInvalidMessages<TEntity>(TEntity item) where TEntity : class
        {
            if (item == null)
                //throw new ArgumentNullException(string.Format(Resource.Exception_NullEntityForValidation, typeof(TEntity)));
                throw new ArgumentNullException();

            var validationErrors = new List<string>();

            SetValidatableObjectErrors(item, validationErrors);
            SetValidationAttributeErrors(item, validationErrors);


            return validationErrors;
        }

        #endregion
    }
}
