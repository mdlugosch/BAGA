using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation
{
    public class BusinessValidations
    {
        public static ValidationResult DescriptionRules(string value)
        {
            var errors = new System.Text.StringBuilder();
            if (value != null)
            {
                var description = (string)value;

                if (description.Contains("!"))
                {
                    errors.AppendLine("Description should not contain '!'.");
                }
                if (description.Contains(":)") || description.Contains(":("))
                {
                    errors.AppendLine("description should not contain emoticons.");
                }
            }
            if (errors.Length > 0) return new ValidationResult(errors.ToString());
            else return ValidationResult.Success;
        }
    }
}
