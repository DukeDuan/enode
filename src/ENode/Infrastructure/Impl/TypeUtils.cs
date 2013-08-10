﻿using System;
using System.ComponentModel;
using System.Linq;
using ENode.Domain;

namespace ENode.Infrastructure {
    public sealed class TypeUtils {
        /// <summary>Convert the given object to a given strong type.
        /// </summary>
        public static T ConvertType<T>(object value) {
            if (value == null) {
                return default(T);
            }
            TypeConverter typeConverter1 = TypeDescriptor.GetConverter(typeof(T));
            TypeConverter typeConverter2 = TypeDescriptor.GetConverter(value.GetType());
            if (typeConverter1.CanConvertFrom(value.GetType())) {
                return (T)typeConverter1.ConvertFrom(value);
            }
            else if (typeConverter2.CanConvertTo(typeof(T))) {
                return (T)typeConverter2.ConvertTo(value, typeof(T));
            }
            else {
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }
        /// <summary>Check whether a type is a component type.
        /// </summary>
        public static bool IsComponent(Type type) {
            return type != null && type.IsClass && type.GetCustomAttributes(typeof(ComponentAttribute), false).Any();
        }
        /// <summary>Check whether a type is an aggregate root type.
        /// </summary>
        public static bool IsAggregateRoot(Type type) {
            return type.IsClass && !type.IsAbstract && typeof(AggregateRoot).IsAssignableFrom(type);
        }
    }
}