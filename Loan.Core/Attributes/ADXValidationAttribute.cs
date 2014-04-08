using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Loan.Core.Attributes
{
    #region 验证基类
    /// <summary>
    /// 通用验证基类
    /// </summary>
    public abstract class ADXValidationAttribute : ValidationAttribute
    {
        #region Constructors
        public ADXValidationAttribute(MessageType messageId, params object[] args) :
            base(() => MessageManager.Current.GetMessage(messageId, args)) { }
        #endregion

        #region Protected Properties
        protected virtual Regex rLetters { get { return new Regex("[a-zA-Z]{1,}"); } }
        /// <summary>
        /// 验证数字
        /// 子类可以根据自己的逻辑去重写
        /// </summary>
        protected virtual Regex rDigit { get { return new Regex("[0-9]{1,}"); } }
        /// <summary>
        /// 验证邮编
        /// 子类可以根据自己的逻辑去重写
        /// </summary>
        protected virtual Regex rPostNumber { get { return new Regex("^[0-9]{3,14}$"); } }
        /// <summary>
        /// 验证手机
        /// 子类可以根据自己的逻辑去重写
        /// </summary>
        protected virtual Regex rMobile { get { return new Regex(@"^1[3|4|5|8][0-9]\d{8}$"); } }
        /// <summary>
        /// 验证电话
        /// 子类可以根据自己的逻辑去重写
        /// </summary>
        protected virtual Regex rTelePhone { get { return new Regex(@"^[0-9]{2,4}-\d{6,8}$"); } }
        /// <summary>
        /// 验证传真
        /// 子类可以根据自己的逻辑去重写
        /// </summary>
        protected virtual Regex rFex { get { return new Regex(@"/^[0-9]{2,4}-\d{6,8}$"); } }
        /// <summary>
        /// 验证Email
        /// 子类可以根据自己的逻辑去重写
        /// </summary>
        protected virtual Regex rEmail { get { return new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"); } }
        #endregion

    }
    #endregion

    public class AgeRangeAttribute : RangeAttribute, IClientValidatable
    {
        public AgeRangeAttribute(int minimum, int maximum)
            : base(minimum, maximum)
        { }

        public override bool IsValid(object value)
        {
            DateTime birthDate = (DateTime)value;
            DateTime age = new DateTime(DateTime.Now.Ticks - birthDate.Ticks);
            return age.Year >= (int)this.Minimum && age.Year <= (int)this.Maximum;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage("年龄");
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule validationRule = new ModelClientValidationRule() { ValidationType = "agerange", ErrorMessage = FormatErrorMessage(metadata.DisplayName) };
            validationRule.ValidationParameters.Add("currentdate", DateTime.Today.ToString("dd-MM-yyyy"));
            validationRule.ValidationParameters.Add("minage", this.Minimum);
            validationRule.ValidationParameters.Add("maxage", this.Maximum);
            yield return validationRule;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ADXCompareAttribute : ValidationAttribute, IClientValidatable
    {
        public string SourceProperty { get; private set; }
        public string OriginalProperty { get; private set; }
        public ValidationCompareOperator Operator { get; private set; }
        public ValidationDataType Type { get; private set; }

        private const string _defaultErrorMessage = "'{0}' 与 '{1}' 进行 {2} 比较失败";

        public ADXCompareAttribute(string sourceProperty, string originalProperty, ValidationCompareOperator op, ValidationDataType type)
            : base(_defaultErrorMessage)
        {
            SourceProperty = sourceProperty;
            OriginalProperty = originalProperty;
            Operator = op;
            Type = type;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                SourceProperty, OriginalProperty, Operator.ToString());
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object sourceProperty = properties.Find(SourceProperty, true /* ignoreCase */).GetValue(value);
            object originalProperty = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            return compare(sourceProperty, originalProperty);
        }

        private bool compare(object sourceProperty, object originalProperty)
        {
            int num = 0;
            switch (this.Type)
            {
                case ValidationDataType.String:
                    num = string.Compare((string)sourceProperty, (string)originalProperty, false, CultureInfo.CurrentCulture);
                    break;

                case ValidationDataType.Integer:
                    num = ((int)sourceProperty).CompareTo(originalProperty);
                    break;

                case ValidationDataType.Double:
                    num = ((double)sourceProperty).CompareTo(originalProperty);
                    break;

                case ValidationDataType.Date:
                    num = ((DateTime)sourceProperty).CompareTo(originalProperty);
                    break;

                case ValidationDataType.Currency:
                    num = ((decimal)sourceProperty).CompareTo(originalProperty);
                    break;
            }
            switch (this.Operator)
            {
                case ValidationCompareOperator.Equal:
                    return (num == 0);

                case ValidationCompareOperator.NotEqual:
                    return (num != 0);

                case ValidationCompareOperator.GreaterThan:
                    return (num > 0);

                case ValidationCompareOperator.GreaterThanEqual:
                    return (num >= 0);

                case ValidationCompareOperator.LessThan:
                    return (num < 0);

                case ValidationCompareOperator.LessThanEqual:
                    return (num <= 0);
            }
            return true;

        }

        #region IClientValidatable 成员

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule validationRule = new ModelClientValidationRule()
            {
                ValidationType = "ADXCompare",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };
            validationRule.ValidationParameters.Add("SourceProperty", this.SourceProperty);
            validationRule.ValidationParameters.Add("OriginalProperty", this.OriginalProperty);
            validationRule.ValidationParameters.Add("Operator", this.Operator);
            validationRule.ValidationParameters.Add("Type", this.Type);
            yield return validationRule;
        }

        #endregion
    }



    #region 具体验证模块
    /// <summary>
    /// 为空验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ADXRequiredAttribute : ADXValidationAttribute
    {
        public bool AllowEmptyStrings { get; set; }
        public ADXRequiredAttribute(MessageType messageType, params object[] args) :
            base(messageType, args)
        { }
        public override bool IsValid(object value)
        {
            return new System.ComponentModel.DataAnnotations.RequiredAttribute { AllowEmptyStrings = this.AllowEmptyStrings }.IsValid(value);
        }
    }
    /// <summary>
    /// 范围验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ADXRangeAttribute : ADXValidationAttribute
    {
        private System.ComponentModel.DataAnnotations.RangeAttribute innerRangeAttribute;

        public ADXRangeAttribute(double minimum, double maximum, MessageType messageType, params object[] args) :
            base(messageType, args)
        {
            innerRangeAttribute = new System.ComponentModel.DataAnnotations.RangeAttribute(minimum, maximum);
        }

        public ADXRangeAttribute(int minimum, int maximum, MessageType messageType, params object[] args) :
            base(messageType, args)
        {
            innerRangeAttribute = new System.ComponentModel.DataAnnotations.RangeAttribute(minimum, maximum);
        }

        public ADXRangeAttribute(Type type, string minimum, string maximum, MessageType messageType, params object[] args) :
            base(messageType, args)
        {
            innerRangeAttribute = new System.ComponentModel.DataAnnotations.RangeAttribute(type, minimum, maximum);
        }

        public override bool IsValid(object value)
        {
            return innerRangeAttribute.IsValid(value);
        }
    }

    /// <summary>
    /// Email验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class EmailAttribute : ADXValidationAttribute
    {
        public EmailAttribute(MessageType messageType, params object[] args) :
            base(messageType, args) { }
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            else
                return rEmail.IsMatch(value.ToString());
        }
    }

    /// <summary>
    /// 数值验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DigitAttribute : ADXValidationAttribute
    {
        public DigitAttribute(MessageType messageType, params object[] args) :
            base(messageType, args) { }
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            else
                return rDigit.IsMatch(value.ToString());
        }

    }

    /// <summary>
    /// 邮编验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class PostNumberAttribute : ADXValidationAttribute
    {
        public PostNumberAttribute(MessageType messageType, params object[] args) :
            base(messageType, args) { }
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            else
                return rPostNumber.IsMatch(value.ToString());
        }

    }

    /// <summary>
    /// 手机验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class MobileAttribute : ADXValidationAttribute
    {
        public MobileAttribute(MessageType messageType, params object[] args) :
            base(messageType, args) { }
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            else
                return rMobile.IsMatch(value.ToString());
        }
    }

    /// <summary>
    /// 电话验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class TelePhoneAttribute : ADXValidationAttribute
    {
        public TelePhoneAttribute(MessageType messageType, params object[] args) :
            base(messageType, args) { }
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            else
                return rTelePhone.IsMatch(value.ToString());
        }
    }

    /// <summary>
    /// 传真验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class FexAttribute : ADXValidationAttribute
    {
        public FexAttribute(MessageType messageType, params object[] args) :
            base(messageType, args) { }
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            else
                return rFex.IsMatch(value.ToString());
        }
    }
    #endregion

    #region 验证消息返回类
    /// <summary>
    /// 消息类
    /// </summary>
    public class MessageManager
    {
        static Dictionary<MessageType, string> messages = new Dictionary<MessageType, string>();
        static MessageManager()
        {
            messages.Add(MessageType.RequiredField, "\"{0}\"不能为空!");
            messages.Add(MessageType.GreaterThan, "这个 \"{0}\" 的值必须大于 \"{1}\"!");
            messages.Add(MessageType.LessThan, "这个 \"{0}\" 的值必须小于 \"{1}\"!");
            messages.Add(MessageType.EmailField, "这个 \"{0}\" 不是有效的Email地址!");
            messages.Add(MessageType.DigitField, "这个 \"{0}\" 不是有效的数字!");
            messages.Add(MessageType.PostNumberField, "这个 \"{0}\" 不是有效的邮编!");
            messages.Add(MessageType.MobileField, "这个 \"{0}\" 不是有效的手机号码!");
            messages.Add(MessageType.TelePhoneField, "这个 \"{0}\" 不是有效的电话号码!");
            messages.Add(MessageType.FexField, "这个 \"{0}\" 不是有效的传真!");
        }
        /// <summary>
        /// 得到验证异常的消息集合
        /// 对外公开
        /// </summary>
        /// <param name="messageType">异常消息ID</param>
        /// <param name="args">消息参数集合</param>
        /// <returns></returns>
        public string GetMessage(MessageType messageType, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, messages[messageType], args);
        }
        /// <summary>
        /// 本类的实例对象
        /// </summary>
        public static MessageManager Current = new MessageManager();
    }



    #endregion

    #region 验证类型枚举
    /// <summary>
    /// 验证消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 为空验证
        /// </summary>
        RequiredField,
        /// <summary>
        /// 大于验证
        /// </summary>
        GreaterThan,
        /// <summary>
        /// 小于验证
        /// </summary>
        LessThan,
        /// <summary>
        /// 邮箱验证
        /// </summary>
        EmailField,
        /// <summary>
        /// 数字验证
        /// </summary>
        DigitField,
        /// <summary>
        /// 邮编验证
        /// </summary>
        PostNumberField,
        /// <summary>
        /// 手机验证
        /// </summary>
        MobileField,
        /// <summary>
        /// 电话验证
        /// </summary>
        TelePhoneField,
        /// <summary>
        /// 传真验证
        /// </summary>
        FexField,
    }
    #endregion
}
