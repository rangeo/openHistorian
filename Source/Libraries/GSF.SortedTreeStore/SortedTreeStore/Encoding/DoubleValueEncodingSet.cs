﻿//******************************************************************************************************
//  DoubleValueEncodingSet.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
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
//  2/22/2014 - Steven E. Chisholm
//       Generated original version of source code. 
//     
//******************************************************************************************************

using GSF.IO;

namespace GSF.SortedTreeStore.Encoding
{
    public class DoubleValueEncodingSet<TKey, TValue>
        : DoubleValueEncodingBase<TKey, TValue>
    {
        SingleValueEncodingBase<TKey> m_keyEncoding;
        SingleValueEncodingBase<TValue> m_valueEncoding;
        public DoubleValueEncodingSet(SingleValueEncodingBase<TKey> keyEncoding, SingleValueEncodingBase<TValue> valueEncoding)
        {
            m_keyEncoding = keyEncoding;
            m_valueEncoding = valueEncoding;
        }

        public override bool UsesPreviousKey
        {
            get
            {
                return m_keyEncoding.UsesPreviousValue;
            }
        }

        public override bool UsesPreviousValue
        {
            get
            {
                return m_valueEncoding.UsesPreviousValue;
            }
        }

        public override int MaxCompressionSize
        {
            get
            {
                return m_keyEncoding.MaxCompressionSize + m_valueEncoding.MaxCompressionSize;
            }
        }

        public override void Compress(BinaryStreamBase stream, TKey prevKey, TValue prevValue, TKey key, TValue value)
        {
            m_keyEncoding.Compress(stream, prevKey, key);
            m_valueEncoding.Compress(stream, prevValue, value);
        }

        public override void Decompress(BinaryStreamBase stream, TKey prevKey, TValue prevValue, TKey key, TValue value)
        {
            m_keyEncoding.Decompress(stream, prevKey, key);
            m_valueEncoding.Decompress(stream, prevValue, value);
        }

        public override DoubleValueEncodingBase<TKey, TValue> Clone()
        {
            return new DoubleValueEncodingSet<TKey, TValue>(m_keyEncoding.Clone(), m_valueEncoding.Clone());
        }
    }
}