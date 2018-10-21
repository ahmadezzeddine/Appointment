using System;
using System.ComponentModel.DataAnnotations;

namespace App.Schedule.Domains.Attributes
{
    public enum CompareDateOperator
    {
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }

    //public sealed class CompareDateAttributes : ValidationAttribute, IClientValidatable
    public sealed class CompareDateAttributes : ValidationAttribute
    {
        private CompareDateOperator operatorname = CompareDateOperator.GreaterThanOrEqual;

        public string CompareToPropertyName { get; set; }
        public CompareDateOperator OperatorName { get { return operatorname; } set { operatorname = value; } }
        // public IComparable CompareDataType { get; set; }

        public CompareDateAttributes() : base() { }
        //Override IsValid
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string operstring = (OperatorName == CompareDateOperator.GreaterThan ?
            "greater than " : (OperatorName == CompareDateOperator.GreaterThanOrEqual ?
            "greater than or equal to " :
            (OperatorName == CompareDateOperator.LessThan ? "less than " :
            (OperatorName == CompareDateOperator.LessThanOrEqual ? "less than or equal to " : ""))));
            var basePropertyInfo = validationContext.ObjectType.GetProperty(CompareToPropertyName);

            var valOther = (IComparable)basePropertyInfo.GetValue(validationContext.ObjectInstance, null);

            var valThis = (IComparable)value;

            if ((operatorname == CompareDateOperator.GreaterThan && valThis.CompareTo(valOther) <= 0) ||
                (operatorname == CompareDateOperator.GreaterThanOrEqual && valThis.CompareTo(valOther) < 0) ||
                (operatorname == CompareDateOperator.LessThan && valThis.CompareTo(valOther) >= 0) ||
                (operatorname == CompareDateOperator.LessThanOrEqual && valThis.CompareTo(valOther) > 0))
                return new ValidationResult(base.ErrorMessage);
            return null;
        }
        #region IClientValidatable Members

        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
        //    ModelClientValidationRule compareRule = new ModelClientValidationRule();
        //    compareRule.ErrorMessage = errorMessage;
        //    compareRule.ValidationType = "genericcompare";
        //    compareRule.ValidationParameters.Add("comparetopropertyname", CompareToPropertyName);
        //    compareRule.ValidationParameters.Add("operatorname", OperatorName.ToString());
        //    yield return compareRule;
        //}

        #endregion
    }
}
