namespace IkariDoTrainingBackend.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            int x = 5;
            int y = 10;

            // Act
            int result = x + y;

            //Assert
            Assert.Equal(15, result);
        }
    }
}