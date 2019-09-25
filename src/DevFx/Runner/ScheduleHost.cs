﻿/* Copyright(c) 2005-2011 R2@DevFx.NET, License(LGPL) */

using DevFx.Logging;
using System;

namespace DevFx.Runner
{
	internal class ScheduleHost : ScheduleRunner
	{
		public ScheduleHost(double interval, DateTime startTime, DateIntervalType intervalType, int intervalValue, IServiceHandler serviceHandler, ILogService logService) : base(interval, startTime, intervalType, intervalValue) {
			this.LogService = logService;
			this.serviceHandler = serviceHandler;
		}
		private readonly IServiceHandler serviceHandler;

		#region Overrides of TimerBase

		protected override bool Enabled {
			get { return base.Enabled; }
			set {
				base.Enabled = value;
				if(this.serviceHandler != null) {
					this.serviceHandler.Enabled = value;
				}
			}
		}

		/// <summary>
		/// 时间到了后引发的操作
		/// </summary>
		protected override void OnTimer() {
			if(this.serviceHandler != null) {
				this.serviceHandler.OnHandle(null);
			}
		}

		#endregion
	}
}
