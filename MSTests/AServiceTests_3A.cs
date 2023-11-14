using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sample;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MSTests
{
    /// <summary>
    /// 調整測試架構 (MSTest + Moq)
    /// </summary>
    [TestClass()]
    public class AServiceTests_3A
    {
        private Mock<IARepository> _aRepository;

        /// <summary>
        /// 初始化
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _aRepository = new Mock<IARepository>();
        }

        /// <summary>
        /// 建立 AccountService 的實體
        /// </summary>
        /// <returns>AccountService 的實體</returns>
        private IAService CreateService()
        {
            return new AService(
                this._aRepository.Object);
        }

        [TestMethod()]
        public void GetMemberCountByVIPLevel_V1_測試情境_檢查回傳結果()
        {
            // Arrange
            this._aRepository.Setup(x => x.GetMemberCount(It.IsAny<int>()))
                             .Returns(10);

            // Act
            var result = this.CreateService().GetMemberCountByVIPLevel(vipLevel: 1);

            // Assert
            this._aRepository
                .Verify(mock => mock.GetMemberCount(It.Is<int>(x => x == 1)), Times.Once);

            Assert.AreEqual(11, result);
        }

        [TestMethod()]
        [DataRow(10, 11, DisplayName = "GetMemberCountByVIPLevel_V2_測試情境1_檢查回傳結果")]
        [DataRow(1000, 1001, DisplayName = "GetMemberCountByVIPLevel_V2_測試情境2_檢查回傳結果")]
        public void GetMemberCountByVIPLevel_V2(int mockMemberCount, int expectMemberCount)
        {
            // Arrange
            this._aRepository.Setup(x => x.GetMemberCount(It.IsAny<int>()))
                             .Returns(mockMemberCount);

            // Act
            var result = this.CreateService().GetMemberCountByVIPLevel(vipLevel: 1);

            // Assert
            this._aRepository
                .Verify(mock => mock.GetMemberCount(It.Is<int>(x => x == 1)), Times.Once);

            Assert.AreEqual(expectMemberCount, result);
        }

        /// <summary>
        /// https://github.com/microsoft/testfx-docs/blob/main/RFCs/006-DynamicData-Attribute.md
        /// https://stackoverflow.com/questions/70589670/c-sharp-unit-test-dynamicdatadisplayname-using-mstest
        /// </summary>
        [TestMethod()]
        [DynamicData(nameof(GetMemberCountByVIPLevel_V3_TestCase), DynamicDataDisplayName = nameof(GetTestDisplayNames))]
        public void GetMemberCountByVIPLevel_V3(int mockMemberCount, int expectMemberCount, string _)
        {
            // Arrange
            this._aRepository.Setup(x => x.GetMemberCount(It.IsAny<int>()))
                             .Returns(mockMemberCount);

            // Act
            var result = this.CreateService().GetMemberCountByVIPLevel(vipLevel: 1);

            // Assert
            this._aRepository
                .Verify(mock => mock.GetMemberCount(It.Is<int>(x => x == 1)), Times.Once);

            Assert.AreEqual(expectMemberCount, result);
        }

        private static IEnumerable<object[]> GetMemberCountByVIPLevel_V3_TestCase
        {
            get
            {
                return new[]
                {
                    new object[] { 10, 11, "測試情境1_檢查回傳結果" },
                    new object[] { 1000, 1001, "測試情境2_檢查回傳結果" },
                    /* .. */
                };
            }
        }

        public static string GetTestDisplayNames(MethodInfo methodInfo, object[] values) => $"{methodInfo.Name}_{values.LastOrDefault()}";

        [TestMethod()]
        [DataRow(PlatformEnum.Web, 1, 0, DisplayName = "GetMemberCountByPlatform_V2_查詢Web玩家數量_檢查回傳結果")]
        [DataRow(PlatformEnum.Mobile, 0, 1, DisplayName = "GetMemberCountByPlatform_V2_查詢Mobile玩家數量_檢查回傳結果")]
        public void GetMemberCountByPlatform_V2(
            PlatformEnum platform,
            int getMemberWebCountReceived,
            int getMemberMobileCountReceived)
        {
            // Arrange
            var expectMemberCount = 1;

            this._aRepository.Setup(x => x.GetMemberWebCount()).Returns(1);
            this._aRepository.Setup(x => x.GetMemberMobileCount()).Returns(1);

            // Act
            var result = this.CreateService().GetMemberCountByPlatform(platform);

            // Assert
            this._aRepository
                .Verify(mock => mock.GetMemberWebCount(), Times.Exactly(getMemberWebCountReceived));

            this._aRepository
                .Verify(mock => mock.GetMemberMobileCount(), Times.Exactly(getMemberMobileCountReceived));

            Assert.AreEqual(expectMemberCount, result);
        }
    }
}