using System;
using System.Collections.Generic;
using System.Text;

namespace MSTests.Helper
{
    internal interface ITestCase<Target, Input, Output>
    {
        Input Arg { get; }
        Target Setup();
        void Check(Output actual);
    }
}
