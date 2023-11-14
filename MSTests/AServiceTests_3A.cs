using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sample;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MSTests
{
    /// <summary>
    /// �վ���լ[�c (MSTest + Moq)
    /// </summary>
    [TestClass()]
    public class AServiceTests_3A
    {
        private Mock<IARepository> _aRepository;

        /// <summary>
        /// ��l��
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _aRepository = new Mock<IARepository>();
        }

        /// <summary>
        /// �إ� AccountService ������
        /// </summary>
        /// <returns>AccountService ������</returns>
        private IAService CreateService()
        {
            return new AService(
                this._aRepository.Object);
        }

        [TestMethod()]
        public void GetMemberCountByVIPLevel_V1_���ձ���_�ˬd�^�ǵ��G()
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
        [DataRow(10, 11, DisplayName = "GetMemberCountByVIPLevel_V2_���ձ���1_�ˬd�^�ǵ��G")]
        [DataRow(1000, 1001, DisplayName = "GetMemberCountByVIPLevel_V2_���ձ���2_�ˬd�^�ǵ��G")]
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
                    new object[] { 10, 11, "���ձ���1_�ˬd�^�ǵ��G" },
                    new object[] { 1000, 1001, "���ձ���2_�ˬd�^�ǵ��G" },
                    /* .. */
                };
            }
        }

        public static string GetTestDisplayNames(MethodInfo methodInfo, object[] values) => $"{methodInfo.Name}_{values.LastOrDefault()}";

        [TestMethod()]
        [DataRow(PlatformEnum.Web, 1, 0, DisplayName = "GetMemberCountByPlatform_V2_�d��Web���a�ƶq_�ˬd�^�ǵ��G")]
        [DataRow(PlatformEnum.Mobile, 0, 1, DisplayName = "GetMemberCountByPlatform_V2_�d��Mobile���a�ƶq_�ˬd�^�ǵ��G")]
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