﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogicUtility.Nodes
{
    public class FirstScoreSelector<T> : ISelector<T> where T : class, IContext
    {
        public INode<T> Next { get; set; }
        public List<IQualifier<T>> Qualifiers { get; } = new List<IQualifier<T>>();

        public virtual string GetLog() => GetType().Name;


        public virtual async Task<INode<T>> SelectAsync(T context)
        {
            for (var i = 0; i < Qualifiers.Count; i++)
            {
                var qualifier = Qualifiers[i];
                var qualifierScore = await qualifier.ScoreAsync(context);
                if (qualifierScore > 0)
                    return qualifier;
            }
            return Next;
        }
    }
}