﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DistributedSystems.WorkerManager
{
    internal class WorkerQueueEvaluator
    {
        internal async static Task<WorkerAction> AdviseAction(int activeWorkerCount, long queueMessageCount, int targetSla, HttpClient httpClient)
        {
            if (activeWorkerCount == 0 && queueMessageCount > 0) return WorkerAction.Add;
            if (activeWorkerCount > 1 && queueMessageCount == 0) return WorkerAction.Remove;

            var result = await (await httpClient.GetAsync("Data/ImageProcessingTime")).Content.ReadAsStringAsync();
            var timespan = new List<TimeSpan>() { JsonConvert.DeserializeObject<TimeSpan>(result) };
            timespan.Add(JsonConvert.DeserializeObject<TimeSpan>(await (await httpClient.GetAsync("Data/CompoundImageProcessingTime")).Content.ReadAsStringAsync()));
            var averageProcessingTime = new TimeSpan(Convert.ToInt64(timespan.Average(time => time.Ticks))).TotalSeconds;

            if (averageProcessingTime > targetSla + 1) return WorkerAction.Add;
            if (averageProcessingTime < targetSla - 1 && activeWorkerCount > 1) return WorkerAction.Remove;

            return WorkerAction.Nothing;
        }
    }
}
