﻿//******************************************************************************************************
//  GetPointStream.cs - Gbtc
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  8/7/2013 - Steven E. Chisholm
//       Generated original version of source code. 
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using GSF.Collections;
using GSF.SortedTreeStore;
using GSF.SortedTreeStore.Engine;
using GSF.SortedTreeStore.Engine.Reader;
using GSF.SortedTreeStore.Filters;
using openHistorian.Collections;
using GSF.SortedTreeStore.Tree;

namespace openHistorian.Data.Query
{

    public class PointStream
            : TreeStream<HistorianKey, HistorianValue>, IDisposable
    {
        SortedTreeEngineReaderBase<HistorianKey, HistorianValue> m_reader;
        TreeStream<HistorianKey, HistorianValue> m_stream;

        public PointStream(SortedTreeEngineReaderBase<HistorianKey, HistorianValue> reader, TreeStream<HistorianKey, HistorianValue> stream)
        {
            m_stream = stream;
            m_reader = reader;
            SetKeyValueReferences(m_stream.CurrentKey, m_stream.CurrentValue);
        }

        public void Dispose()
        {
            m_stream.Cancel();
            m_reader.Dispose();
        }

        public override bool Read()
        {
            if (m_stream.Read())
            {
                IsValid = true;
                return true;
            }
            else
            {
                IsValid = false;
                return false;
            }
        }
    }

    /// <summary>
    /// Queries a historian database for a set of signals. 
    /// </summary>
    public static partial class GetPointStreamExtensionMethods
    {


        ///// <summary>
        ///// Gets frames from the historian as individual frames.
        ///// </summary>
        ///// <param name="database">the database to use</param>
        ///// <returns></returns>
        //public static SortedList<DateTime, FrameData> GetFrames(this SortedTreeEngineBase<HistorianKey, HistorianValue> database, DateTime timestamp)
        //{
        //    return database.GetFrames(SortedTreeEngineReaderOptions.Default, TimestampFilter.CreateFromRange<HistorianKey>(timestamp, timestamp), PointIDFilter.CreateAllKeysValid<HistorianKey>(), null);
        //}

        /// <summary>
        /// Gets frames from the historian as individual frames.
        /// </summary>
        /// <param name="database">the database to use</param>
        /// <returns></returns>
        public static PointStream GetPointStream(this SortedTreeEngineBase<HistorianKey, HistorianValue> database, DateTime startTime, DateTime stopTime)
        {
            return database.GetPointStream(SortedTreeEngineReaderOptions.Default, TimestampFilter.CreateFromRange<HistorianKey>(startTime, stopTime), null, null);
        }

        /// <summary>
        /// Gets frames from the historian as individual frames.
        /// </summary>
        /// <param name="database">the database to use</param>
        /// <returns></returns>
        public static PointStream GetPointStream(this SortedTreeEngineBase<HistorianKey, HistorianValue> database, KeySeekFilterBase<HistorianKey> timestamps, params ulong[] points)
        {
            return database.GetPointStream(SortedTreeEngineReaderOptions.Default, timestamps, PointIDFilter.CreateFromList<HistorianKey>(points), null);
        }

        /// <summary>
        /// Gets frames from the historian as individual frames.
        /// </summary>
        /// <param name="database">the database to use</param>
        /// <returns></returns>
        public static PointStream GetPointStream(this SortedTreeEngineBase<HistorianKey, HistorianValue> database, DateTime startTime, DateTime stopTime, params ulong[] points)
        {
            return database.GetPointStream(SortedTreeEngineReaderOptions.Default, TimestampFilter.CreateFromRange<HistorianKey>(startTime, stopTime), PointIDFilter.CreateFromList<HistorianKey>(points), null);
        }

        ///// <summary>
        ///// Gets frames from the historian as individual frames.
        ///// </summary>
        ///// <param name="database">the database to use</param>
        ///// <returns></returns>
        //public static SortedList<DateTime, FrameData> GetFrames(this SortedTreeEngineBase<HistorianKey, HistorianValue> database)
        //{
        //    return database.GetFrames(QueryFilterTimestamp.CreateAllKeysValid(), QueryFilterPointId.CreateAllKeysValid(), SortedTreeEngineReaderOptions.Default);
        //}

        ///// <summary>
        ///// Gets frames from the historian as individual frames.
        ///// </summary>
        ///// <param name="database">the database to use</param>
        ///// <param name="timestamps">the timestamps to query for</param>
        ///// <returns></returns>
        //public static SortedList<DateTime, FrameData> GetFrames(this SortedTreeEngineBase<HistorianKey, HistorianValue> database, QueryFilterTimestamp timestamps)
        //{
        //    return database.GetFrames(timestamps, QueryFilterPointId.CreateAllKeysValid(), SortedTreeEngineReaderOptions.Default);
        //}

        /// <summary>
        /// Gets frames from the historian as individual frames.
        /// </summary>
        /// <param name="database">the database to use</param>
        /// <param name="timestamps">the timestamps to query for</param>
        /// <param name="points">the points to query</param>
        /// <returns></returns>
        public static PointStream GetPointStream(this SortedTreeEngineBase<HistorianKey, HistorianValue> database, KeySeekFilterBase<HistorianKey> timestamps, KeyMatchFilterBase<HistorianKey> points)
        {
            return database.GetPointStream(SortedTreeEngineReaderOptions.Default, timestamps, points, null);
        }

        /// <summary>
        /// Gets frames from the historian as individual frames.
        /// </summary>
        /// <param name="database">the database to use</param>
        /// <param name="timestamps">the timestamps to query for</param>
        /// <param name="points">the points to query</param>
        /// <param name="options">A list of query options</param>
        /// <returns></returns>
        public static PointStream GetPointStream(this SortedTreeEngineBase<HistorianKey, HistorianValue> database,
            SortedTreeEngineReaderOptions options, KeySeekFilterBase<HistorianKey> timestamps, KeyMatchFilterBase<HistorianKey> points,
            ValueMatchFilterBase<HistorianValue> value)
        {
            var reader = database.OpenDataReader();
            return new PointStream(reader, reader.Read(options, timestamps, points, value));
        }



        class FrameDataConstructor
        {
            public List<ulong> PointId = new List<ulong>();
            public List<HistorianValueStruct> Values = new List<HistorianValueStruct>();
            public FrameData ToFrameData()
            {
                return new FrameData(PointId, Values);
            }
        }

        /// <summary>
        /// Gets concentrated frames from the provided stream
        /// </summary>
        /// <param name="stream">the database to use</param>
        /// <returns></returns>
        public static SortedList<DateTime, FrameData> GetPointStream(this TreeStream<HistorianKey, HistorianValue> stream)
        {
            SortedList<DateTime, FrameDataConstructor> results = new SortedList<DateTime, FrameDataConstructor>();
            ulong lastTime = ulong.MinValue;
            FrameDataConstructor lastFrame = null;
            while (stream.Read())
            {
                if (lastFrame == null || stream.CurrentKey.Timestamp != lastTime)
                {
                    lastTime = stream.CurrentKey.Timestamp;
                    DateTime timestamp = new DateTime((long)lastTime);

                    if (!results.TryGetValue(timestamp, out lastFrame))
                    {
                        lastFrame = new FrameDataConstructor();
                        results.Add(timestamp, lastFrame);
                    }
                }
                lastFrame.PointId.Add(stream.CurrentKey.PointID);
                lastFrame.Values.Add(stream.CurrentValue.ToStruct());
            }
            List<FrameData> data = new List<FrameData>(results.Count);
            data.AddRange(results.Values.Select(x => x.ToFrameData()));
            return SortedListConstructor.Create(results.Keys, data);
        }


    }
}