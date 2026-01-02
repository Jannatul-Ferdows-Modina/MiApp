using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppMGL.DAL.Helper
{
    /// <summary>
    /// Base class for entities
    /// </summary>
    public abstract class BaseEntity : IValidatableObject
    {
        #region Members

        readonly int? _requestedHashCode = null;

        #endregion

        #region Constructors

        protected BaseEntity()
        {
            
        }

        #endregion

        #region Overrides Methods

        public static bool operator ==(BaseEntity left, BaseEntity right)
        {
            if (Equals(left, null))
                return (Equals(right, null));
            return left.Equals(right);
        }

        public static bool operator !=(BaseEntity left, BaseEntity right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BaseEntity)obj);
        }

        public override int GetHashCode()
        {
            return _requestedHashCode.GetHashCode();
        }

        #endregion

        #region Implemented Methods

        /// <summary>
        /// Determines whether this object is valid or not.
        /// </summary>
        /// <param name="validationContext">Describes the context in which a validation check is performed.</param>
        /// <returns>A IEnumerable of ValidationResult. The IEnumerable is empty when the object is in a valid state.</returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            return validationResults;
        }

        /// <summary>
        /// Determines whether this object is valid or not.
        /// </summary>
        /// <returns>A IEnumerable of ValidationResult. The IEnumerable is empty when the object is in a valid state.</returns>
        public IEnumerable<ValidationResult> Validate()
        {
            var validationErrors = new List<ValidationResult>();
            var ctx = new ValidationContext(this, null, null);
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(this, ctx, validationErrors, true);
            return validationErrors;
        }

        #endregion
    }
}
