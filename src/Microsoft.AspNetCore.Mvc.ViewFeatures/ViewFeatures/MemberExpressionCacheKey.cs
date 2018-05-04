// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures
{
    internal struct MemberExpressionCacheKey
    {
        public MemberExpressionCacheKey(Type modelType, MemberExpression memberExpression)
        {
            ModelType = modelType;
            MemberExpression = memberExpression;
            Members = null;
        }

        public MemberExpressionCacheKey(Type modelType, MemberInfo[] members)
        {
            ModelType = modelType;
            Members = members;
            MemberExpression = null;
        }

        public MemberExpressionCacheKey MakeCacheable()
        {
            var members = new List<MemberInfo>();
            foreach (var member in this)
            {
                members.Add(member);
            }

            return new MemberExpressionCacheKey(ModelType, members.ToArray());
        }

        public MemberExpression MemberExpression { get; }

        public Type ModelType { get; }

        public MemberInfo[] Members { get; }

        public Enumerator GetEnumerator() => new Enumerator(ref this);

        public struct Enumerator
        {
            private readonly MemberInfo[] _members;
            private int _index;
            private MemberExpression _memberExpression;

            public Enumerator(ref MemberExpressionCacheKey key)
            {
                Current = null;
                _members = key.Members;
                _memberExpression = key.MemberExpression;
                _index = -1;
            }

            public MemberInfo Current { get; private set; }

            public bool MoveNext()
            {
                _index++;
                if (_members != null)
                {
                    if (_index >= _members.Length)
                    {
                        return false;
                    }

                    Current = _members[_index];
                    return true;
                }

                if (_memberExpression == null)
                {
                    return false;
                }

                Current = _memberExpression.Member;
                _memberExpression = _memberExpression.Expression as MemberExpression;
                return true;
            }
        }
    }
}
