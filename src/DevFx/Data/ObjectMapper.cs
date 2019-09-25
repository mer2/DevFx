﻿/* Copyright(c) 2005-2011 R2@DevFx.NET, License(LGPL) */

using DevFx.Data.Results;
using DevFx.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DevFx.Data
{
	[Object]
	internal sealed class ObjectMapper : ObjectMapperBase
	{
		#region RefectionMapping

		internal static void RefectionMapping(object instance, IDataRecord dataRecord) {
			if (instance == null) {
				return;
			}
			RefectionMapping(instance, dataRecord.ToDictionary());
		}

		internal static void RefectionMapping(object instance, IDictionary dictionary) {
			if (instance == null) {
				return;
			}
			if(instance is IEntityInitialize) {
				((IEntityInitialize)instance).InitValues(dictionary);
				return;
			}
			var accessors = TypeHelper.GetPropertyAccessors(instance.GetType());
			foreach(var key in dictionary.Keys) {
				if (!(key is string columnName)) {
					continue;
				}
				if (!accessors.TryGetValue(columnName, out var accessor)) {
					continue;
				}
				var columnValue = dictionary[key];
				if (Convert.IsDBNull(columnValue)) {
					if (accessor.Property.MemberType.IsValueType) {
						continue;
					}
					columnValue = null;
				}
				accessor.SetValue(instance, columnValue);
			}
		}

		internal static void RefectionMapping(object instance, IDataRecord dataRecord, IList<string> excludeProperties, IList<string> includeProperties) {
			if (instance == null) {
				return;
			}
			RefectionMapping(instance, dataRecord.ToDictionary(), excludeProperties, includeProperties);
		}

		internal static void RefectionMapping(object instance, IDictionary dictionary, IList<string> excludeProperties, IList<string> includeProperties) {
			if (instance == null || dictionary == null || dictionary.Count <= 0) {
				return;
			}
			var keys = new object[dictionary.Count];
			dictionary.Keys.CopyTo(keys, 0);
			foreach (var key in keys) {
				var columnName = key as string;
				if (columnName == null || excludeProperties.Contains(columnName) || (!includeProperties.Contains("*") && !includeProperties.Contains(columnName))) {
					dictionary.Remove(key);
					continue;
				}
			}
			if (dictionary.Count > 0) {
				RefectionMapping(instance, dictionary);
			}
		}

		#endregion
	}
}
