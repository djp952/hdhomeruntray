//---------------------------------------------------------------------------
// Copyright (c) 2021 Michael G. Brehm
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//---------------------------------------------------------------------------

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class RadiiConverter (internal)
	//
	// TypeConverter for the Radii class so the Windows Forms Designer
	// can work with it
	//
	// Based on .NET Foundation .NET Windows Forms "PaddingConverter":
	// https://github.com/dotnet/winforms
	//
	// Licensed to the .NET Foundation under one or more agreements.
	// The .NET Foundation licenses this file to you under the MIT license.
	// See the LICENSE file in the project root for more information.
	//
	// TODO: Clean this up

	class RadiiConverter : TypeConverter
	{
		/// <summary>
		///  Determines if this converter can convert an object in the given source type to
		///  the native type of the converter.
		/// </summary>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if(sourceType == typeof(string))
			{
				return true;
			}

			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}

			return base.CanConvertTo(context, destinationType);
		}

		/// <summary>
		///  Converts the given object to the converter's native type.
		/// </summary>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if(value is string stringValue)
			{
				stringValue = stringValue.Trim();
				if(stringValue.Length == 0)
				{
					return null;
				}

				// Parse 4 integer values.
				if(culture is null)
				{
					culture = CultureInfo.CurrentCulture;
				}

				string[] tokens = stringValue.Split(new char[] { culture.TextInfo.ListSeparator[0] });
				int[] values = new int[tokens.Length];
				TypeConverter intConverter = TypeDescriptor.GetConverter(typeof(int));
				for(int i = 0; i < values.Length; i++)
				{
					// Note: ConvertFromString will raise exception if value cannot be converted.
					values[i] = (int)intConverter.ConvertFromString(context, culture, tokens[i]);
				}

				if(values.Length != 4)
				{
					throw new ArgumentException(nameof(value));
				}

				return new Radii(values[0], values[1], values[2], values[3]);
			}

			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if(value is Radii radii)
			{
				if(destinationType == typeof(string))
				{
					if(culture is null)
					{
						culture = CultureInfo.CurrentCulture;
					}

					string sep = culture.TextInfo.ListSeparator + " ";
					TypeConverter intConverter = TypeDescriptor.GetConverter(typeof(int));
					// Note: ConvertToString will raise exception if value cannot be converted.
					string[] args = new string[]
					{
						intConverter.ConvertToString(context, culture, radii.TopLeft),
						intConverter.ConvertToString(context, culture, radii.TopRight),
						intConverter.ConvertToString(context, culture, radii.BottomRight),
						intConverter.ConvertToString(context, culture, radii.BottomLeft)
					};
					return string.Join(sep, args);
				}
				else if(destinationType == typeof(InstanceDescriptor))
				{
					if(radii.ShouldSerializeAll())
					{
						return new InstanceDescriptor(
							typeof(Radii).GetConstructor(new Type[] { typeof(int) }),
							new object[] { radii.All }
						);
					}
					else
					{
						return new InstanceDescriptor(
							typeof(Radii).GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) }),
							new object[] { radii.TopLeft, radii.TopRight, radii.BottomRight, radii.BottomLeft }
						);
					}
				}
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			if(propertyValues == null) throw new ArgumentNullException(nameof(propertyValues));

			var original = context.PropertyDescriptor.GetValue(context.Instance);
			try
			{
				// When incrementally changing an existing Radii instance e.g. through a PropertyGrid
				// the expected behavior is that a change of Radii.All will override the now outdated
				// other properties of the original radii.
				//
				// If we are not incrementally changing an existing instance (i.e. have no original value)
				// then we can just select the individual components passed in the full set of properties
				// and the Radii constructor will determine whether they are all the same or not.

				if(original is Radii originalRadii)
				{
					int all = (int)propertyValues[nameof(Radii.All)];
					if(originalRadii.All != all)
					{
						return new Radii(all);
					}
				}

				return new Radii(
					(int)propertyValues[nameof(Radii.TopLeft)],
					(int)propertyValues[nameof(Radii.TopRight)],
					(int)propertyValues[nameof(Radii.BottomRight)],
					(int)propertyValues[nameof(Radii.BottomLeft)]
				);
			}
			catch(InvalidCastException)
			{
				throw new ArgumentException(nameof(propertyValues));
			}
			catch(NullReferenceException)
			{
				throw new ArgumentNullException(nameof(propertyValues));
			}
		}

		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) => true;

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(Radii), attributes);
			return props.Sort(new string[] { nameof(Radii.All), nameof(Radii.TopLeft), nameof(Radii.TopRight), nameof(Radii.BottomRight), nameof(Radii.BottomLeft) });
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context) => true;
	}
}
