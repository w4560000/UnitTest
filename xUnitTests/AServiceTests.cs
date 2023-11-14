using Moq;
using Sample;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Xunit;
using Xunit.Abstractions;

namespace xUnitTests
{
    public class AServiceTests : IDisposable
    {
        private Mock<IARepository> _aRepository;

        public AServiceTests()
        {
            _aRepository = new Mock<IARepository>();
        }

        public void Dispose()
        {
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

        [Fact]
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

            Assert.Equal(11, result);
        }

        [Theory()]
        [InlineData(10, 11, "GetMemberCountByVIPLevel_V2_測試情境1_檢查回傳結果")]
        [InlineData(1000, 1001, "GetMemberCountByVIPLevel_V2_測試情境1_檢查回傳結果")]
        public void GetMemberCountByVIPLevel_V2(int mockMemberCount, int expectMemberCount, string _)
        {
            // Arrange
            this._aRepository.Setup(x => x.GetMemberCount(It.IsAny<int>()))
                             .Returns(mockMemberCount);

            // Act
            var result = this.CreateService().GetMemberCountByVIPLevel(vipLevel: 1);

            // Assert
            this._aRepository
                .Verify(mock => mock.GetMemberCount(It.Is<int>(x => x == 1)), Times.Once);

            Assert.Equal(expectMemberCount, result);
        }


        /// <summary>
        /// https://stackoverflow.com/questions/46749152/xunit-display-test-names-for-theory-memberdata-testcase
        /// </summary>
        [Theory()]
        [ClassData(typeof(TestData))]
        public void GetMemberCountByVIPLevel_V3(GetMemberCountByVIPLevel_V3_Input input)
        {
            // Arrange
            this._aRepository.Setup(x => x.GetMemberCount(It.IsAny<int>()))
                             .Returns(input.MockMemberCount);

            // Act
            var result = this.CreateService().GetMemberCountByVIPLevel(vipLevel: 1);

            // Assert
            this._aRepository
                .Verify(mock => mock.GetMemberCount(It.Is<int>(x => x == 1)), Times.Once);

            Assert.Equal(input.ExpectMemberCount, result);
        }

        public class GetMemberCountByVIPLevel_V3_Input
        {
            public GetMemberCountByVIPLevel_V3_Input(string disPlayName)
            {
                this.DisPlayName = disPlayName;
            }

            private string DisPlayName { get; set; }
            public int MockMemberCount { get; set; }
            public int ExpectMemberCount { get; set; }

            public override string ToString()
            {
                return DisPlayName;
            }
        }

        public class TestData : TheoryData<GetMemberCountByVIPLevel_V3_Input>
        {
            public TestData()
            {
                Add(new GetMemberCountByVIPLevel_V3_Input("GetMemberCountByVIPLevel_V3_測試情境1_檢查回傳結果") { MockMemberCount = 10, ExpectMemberCount = 11 });
                Add(new GetMemberCountByVIPLevel_V3_Input("GetMemberCountByVIPLevel_V3_測試情境2_檢查回傳結果") { MockMemberCount = 1000, ExpectMemberCount = 1001 });
            }
        }
    }
}
