﻿/* Copyright(c) 2005-2011 R2@DevFx.NET, License(LGPL) */

namespace DevFx.Data
{
	[Service]
	public interface IResultModule
	{
		void OnResultExecute(IDbResultContext ctx);
	}
}
