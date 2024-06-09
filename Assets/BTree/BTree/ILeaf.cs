using System;

namespace BTree
{
    public interface ILeaf
    {
        void Enter();
        void Execute();
        void Exit();
        TreeResponse Response { get; }
        void Fail();
        event Func<bool> OnExceptionFail;
    }
}