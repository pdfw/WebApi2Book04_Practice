﻿using log4net;
using System;
using System.Net.Http;
using System.Web.Http.Tracing;
using WebApi2Book.Common.Logging;

namespace WebApi2Book.Web.Common
{
    public class SimpleTraceWriter : ITraceWriter
    {
        private readonly ILog _log;
        
        public SimpleTraceWriter(ILogManager logManager)
        {
            _log = logManager.GetLog(typeof(SimpleTraceWriter));
        }

        public void Trace(HttpRequestMessage request, string category, TraceLevel level, 
            Action<TraceRecord> traceAction)
        {
            var rec = new TraceRecord(request, category, level);
            traceAction(rec);
            WriteTrace(rec);
        }

        public void WriteTrace(TraceRecord rec)
        {
            const string traceFormat =
            " RequestId={0};{1}Kind={2};{3}Status={4};{5}Operation={6};{7}Operator={8};{9}Category={10}{11}Request={12}{13}Message={14}";
            var args = new object[]
            {
                rec.RequestId,
                Environment.NewLine,
                rec.Kind,
                Environment.NewLine,
                rec.Status,
                Environment.NewLine,
                rec.Operation,
                Environment.NewLine,
                rec.Operator,
                Environment.NewLine,
                rec.Category,
                Environment.NewLine,
                rec.Request,
                Environment.NewLine,
                rec.Message
            };
            switch (rec.Level)
            {
                case TraceLevel.Debug:
                    _log.DebugFormat(traceFormat, args);
                    _log.DebugFormat(Environment.NewLine);
                    break;
                case TraceLevel.Info:
                    _log.InfoFormat(traceFormat, args);
                    _log.InfoFormat(Environment.NewLine);
                    break;
                case TraceLevel.Warn:
                    _log.WarnFormat(traceFormat, args);
                    _log.WarnFormat(Environment.NewLine);
                    break;

                case TraceLevel.Error:
                    _log.ErrorFormat(traceFormat, args);
                    _log.ErrorFormat(Environment.NewLine);
                    break;
                case TraceLevel.Fatal:
                    _log.FatalFormat(traceFormat, args);
                    _log.FatalFormat(Environment.NewLine);
                    break;
            }
        }
    }
}
