using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sample;
using MSTests.Helper;
using System;
using System.Collections.Generic;

namespace MSTests
{
    /// <summary>
    /// 團隊目前專案寫法
    /// </summary>
    internal abstract class CheckGetMemberCountTestsHelperTestCase
    {
        protected readonly List<Action> _verifyList;

        public CheckGetMemberCountTestsHelperTestCase()
        {
            _verifyList = new List<Action>();
        }

        protected void setup_Success_ARepository(Mock<IARepository> fakeRepository, int vipLevel, int count)
        {
            fakeRepository.Setup(x => x.GetMemberCount(vipLevel)).Returns(count);
            _verifyList.Add(() => { fakeRepository.Verify(x => x.GetMemberCount(vipLevel), Times.Once()); });
        }
    }

    internal class CheckGetMemberCountTests_TestCase1 : CheckGetMemberCountTestsHelperTestCase, ITestCase<AService, object?, int>
    {
        private int _mockMemberCount;
        private int _expectMemberCount;
        public CheckGetMemberCountTests_TestCase1(int mockMemberCount, int expectMemberCount)
            : base()
        {
            _mockMemberCount = mockMemberCount;
            _expectMemberCount = expectMemberCount;
        }

        public object? Arg => null;

        public AService Setup()
        {
            Mock<IARepository> fakeARepository = new Mock<IARepository>();
            setup_Success_ARepository(fakeARepository, 1, _mockMemberCount);
            IARepository mockARepository = fakeARepository.Object;

            return new AService(
                 mockARepository);
        }

        public void Check(int actual)
        {
            _verifyList.ForEach(verify => verify());

            Assert.AreEqual(_expectMemberCount, actual);
        }
    }

    [TestClass()]
    public class AServiceTests_Team
    {
        [TestMethod()]
        public void GetMemberCountTest()
        {
            ITestCase<AService, object?, int>[] testCases = new ITestCase<AService, object?, int>[] {
                new CheckGetMemberCountTests_TestCase1(10, 11),
                new CheckGetMemberCountTests_TestCase1(1000, 1001),
            };

            foreach (ITestCase<AService, object?, int> testCase in testCases)
            {
                AService target = testCase.Setup();
                int output = target.GetMemberCountByVIPLevel(1);
                try
                {
                    testCase.Check(output);
                }
                catch (Exception ex)
                {
                    throw new Exception($"TestCase: {testCase.GetType().Name} {ex})");
                }
            }
        }
    }
}