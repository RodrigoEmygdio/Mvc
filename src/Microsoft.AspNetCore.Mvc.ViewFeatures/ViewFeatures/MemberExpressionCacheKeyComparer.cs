// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures
{
    internal class MemberExpressionCacheKeyComparer : IEqualityComparer<MemberExpressionCacheKey>
    {
        public static readonly MemberExpressionCacheKeyComparer Instance = new MemberExpressionCacheKeyComparer();

        public bool Equals(MemberExpressionCacheKey x, MemberExpressionCacheKey y)
        {
            if (x.ModelType != y.ModelType)
            {
                return false;
            }

            var xEnumerator = x.GetEnumerator();
            var yEnumerator = y.GetEnumerator();

            while (xEnumerator.MoveNext())
            {
                if (!yEnumerator.MoveNext())
                {
                    return false;
                }

                if (xEnumerator.Current != yEnumerator.Current)
                {
                    return false;
                }
            }

            return !yEnumerator.MoveNext();
        }

        public int GetHashCode(MemberExpressionCacheKey obj)
        {
            return obj.Members != null ?
                obj.Members[0].GetHashCode() :
                obj.MemberExpression.Member.GetHashCode();
        }
    }
}
