﻿using System;
using System.ComponentModel;
using System.Reflection;

/// <remarks>
/// Este assembly sobra; metámoslo en MovistarPlus.DaypartSaver.Domain.Model.
/// Hecho
/// </remarks>
namespace MovistarPlus.Common
{
    public class EnumDecorator<T> where T : struct
    {
        private T value;
        public T Value
        {
            get { return this.value; }
        }

        public EnumDecorator(T decoratedEnum)
        {
            this.value = decoratedEnum;
        }
        public EnumDecorator(string description)
        {
            this.value = this.GetValueFromDescription(description);
        }

        public string GetDescription()                
        {
            Type type = this.value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(value.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return value.ToString();
        }
        public override string ToString()
        {
            Type type = this.value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            return value.ToString();
        }
        private T GetValueFromDescription(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                if (field.Name == description)
                    return (T)field.GetValue(null);
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }
    }
}
