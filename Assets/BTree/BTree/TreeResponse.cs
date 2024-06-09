using System;
using System.Collections.Generic;
using System.Linq;

namespace BTree
{
    [Serializable]
    public class TreeResponse : object
    {
        public ILeaf Origin { get; private set; }
        public List<Condition> Conditions { get; private set; }
        public Result Result { get; set; }

        internal TreeResponse(ILeaf origin) : this() { Origin = origin; }
        internal TreeResponse(Result result) : this() { Result = result; }
        private TreeResponse() { Conditions = new List<Condition>(); }

        public bool CheckConditions()
        {
            foreach (var condition in Conditions.Where(condition => !condition.Check()))
            {
                if (condition.Host == null)
                    Result = Result.Failure;
                else
                    condition.Host.RecursiveFail();

                return false;
            }

            return true;
        }
    }

    public enum Result
    {
        Running = 0,
        Success,
        Failure,
    }
}