using Microsoft.QualityTools.Testing.Fakes;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using Pose;
using Sample;
using Sample.Fakes;
//using Sample.Fakes;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace NUnitTests
{
    /// <summary>
    /// NUnit + NSubstitute
    /// </summary>
    [TestFixture()]
    public class AServiceTests
    {
        private IARepository _aRepository;

        /// <summary>
        /// 初始化
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _aRepository = Substitute.For<IARepository>();
        }

        /// <summary>
        /// 建立 AccountService 的實體
        /// </summary>
        /// <returns>AccountService 的實體</returns>
        private IAService CreateService()
        {
            return new AService(
                this._aRepository);
        }

        [Test()]
        public void GetMemberCountByVIPLevel_V1_測試情境_檢查回傳結果()
        {
            // Arrange
            this._aRepository.GetMemberCount(Arg.Any<int>())
                             .Returns(10);

            // Act
            var result = this.CreateService().GetMemberCountByVIPLevel(vipLevel: 1);

            // Assert
            this._aRepository
                .Received(1)
                .GetMemberCount(Arg.Is<int>(x => x == 1));

            Assert.AreEqual(11, result);
        }

        [Test()]
        [Category("TestCategory")]
        [TestCase(10, 11, TestName = "GetMemberCountByVIPLevel_V2_測試情境1_檢查回傳結果")]
        [TestCase(1000, 1001, TestName = "GetMemberCountByVIPLevel_V2_測試情境2_檢查回傳結果")]
        public void GetMemberCountByVIPLevel_V2(int mockMemberCount, long expectMemberCount)
        {
            // Arrange
            this._aRepository.GetMemberCount(Arg.Any<int>())
                             .Returns(mockMemberCount);

            // Act
            var result = this.CreateService().GetMemberCountByVIPLevel(vipLevel: 1);

            // Assert
            this._aRepository
                .Received(1)
                .GetMemberCount(1);

            Assert.AreEqual(expectMemberCount, result);
        }

        /// <summary>
        /// VS 測試總管 抓不到測試方法來源 issue: https://github.com/nunit/nunit3-vs-adapter/issues/721
        /// </summary>
        [Test]
        [TestCaseSource(nameof(TestDataCases))]
        public void GetMemberCountByVIPLevel_V3(int mockMemberCount, int expectMemberCount)
        {
            // Arrange
            this._aRepository.GetMemberCount(Arg.Any<int>())
                             .Returns(mockMemberCount);

            // Act
            var result = this.CreateService().GetMemberCountByVIPLevel(vipLevel: 1);

            // Assert
            this._aRepository
                .Received(1)
                .GetMemberCount(Arg.Is<int>(x => x == 1));

            Assert.AreEqual(expectMemberCount, result);
        }

        public static IEnumerable TestDataCases
        {
            get
            {
                yield return new TestCaseData(10, 11).SetName("GetMemberCountByVIPLevel_V3_測試情境1_檢查回傳結果");
                yield return new TestCaseData(1000, 1001).SetName("GetMemberCountByVIPLevel_V3_測試情境2_檢查回傳結果");
            }
        }

        [Test]
        [TestCase(PlatformEnum.Web, 1, 0, TestName = "GetMemberCountByPlatform_V2_查詢Web玩家數量_檢查回傳結果")]
        [TestCase(PlatformEnum.Mobile, 0, 1, TestName = "GetMemberCountByPlatform_V2_查詢Web玩家數量_檢查回傳結果")]
        public void GetMemberCountByPlatform_V2(
            PlatformEnum platform,
            int getMemberWebCountReceived,
            int getMemberMobileCountReceived)
        {
            // Arrange
            var expectMemberCount = 1;

            this._aRepository.GetMemberWebCount().Returns(1);
            this._aRepository.GetMemberMobileCount().Returns(1);

            // Act
            var result = this.CreateService().GetMemberCountByPlatform(platform);

            // Assert
            this._aRepository
                .Received(getMemberWebCountReceived)
                .GetMemberWebCount();

            this._aRepository
                .Received(getMemberMobileCountReceived)
                .GetMemberMobileCount();

            Assert.AreEqual(expectMemberCount, result);
        }

        [Test()]
        public async Task GetMemberCountByVIPLevelAsync_V1_測試情境_檢查回傳結果()
        {
            // Arrange
            this._aRepository.GetMemberCountAsync(Arg.Any<int>()).Returns(10);

            // Act
            var result = await this.CreateService().GetMemberCountByVIPLevelAsync(vipLevel: 1);

            // Assert
            await this._aRepository
                      .Received(1)
                      .GetMemberCountAsync(Arg.Is<int>(x => x == 1));

            Assert.AreEqual(11, result);
        }

        [Test()]
        public void ThrowException_測試情境_檢查回傳結果()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<Exception>(() => this.CreateService().ThrowException());

            // Assert
            Assert.AreEqual("123", ex.Message);

            // AreEqual = Assert.IsTrue(Object.Equals(a,b)) 根據各自物件的 Equals 定義來判斷
            // AreSame = Assert.IsTrue(Object.RefrenceEquals(a,b)) 比對記憶體位址

            //var a = 1;
            //var b = 1;
            //Assert.AreEqual(a, b); // true
            //Assert.AreSame(a, b); // false

            //var a0 = true;
            //var b0 = true;
            //Assert.AreEqual(a0, b0); // true
            //Assert.AreSame(a0, b0); // false

            //var a1 = new A();
            //var b1 = a1;

            //Assert.AreEqual(a1, b1); // true
            //Assert.AreSame(a1, b1); // true
        }

        [Test()]
        public void GetDatetime_測試Fakes_檢查回傳結果()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2020, 01, 1);

                // Act
                var result = this.CreateService().GetDatetime();

                // Assert
                Assert.AreEqual("2020-01-01", result);
            }
        }

        [Test()]
        public void GetDatetime_測試Fakes_驗證靜態方法()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                ShimHelper.Test = () => "456";

                // Act
                var result = this.CreateService().GetTest();

                // Assert
                Assert.AreEqual("456", result);
            }
        }

        //[Test()]
        //public void GetDatetime_測試Pose檢查回傳結果()
        //{
        //    // Arrange
        //    Shim dateTimeShim = Shim.Replace(() => DateTime.Now).With(() => new DateTime(2020, 01, 1));

        //    PoseContext.Isolate(() =>
        //    {
        //        //// Act
        //        var result = this.CreateService().GetDatetime();

        //        //// Assert
        //        //Assert.AreEqual("2020-01-01", result);

        //    }, dateTimeShim);
        //}
    }
}