﻿/* Copyright(c) 2005-2011 R2@DevFx.NET, License(LGPL) */

using System;
using System.Data;
using System.Data.Common;

namespace DevFx.Data.ResultHandlers
{
	[Object(Name = "Data.DataTableResult"), ResultHandler(typeof(DataTable))]
	public class DataTableResultHandler : ResultHandlerBase
	{
		protected override object ExecuteResultInternal(IDbExecuteContext ctx, DbCommand command, Type resultType, object resultInstance) {
			var defaultType = typeof(DataTable);
			var type = this.GetResultType(ctx, resultType, resultInstance, defaultType);
			if (!defaultType.IsAssignableFrom(type)) {
				throw new DataException("要求返回的类型不正确");
			}
			var provider = ctx.DataStorage.DataProviderFactory;
			var result = resultInstance as DataTable ?? new DataTable();
			using (var adapter = provider.CreateDataAdapter()) {
				adapter.SelectCommand = command;
				adapter.Fill(result);
			}
			return result;
		}
	}
}
