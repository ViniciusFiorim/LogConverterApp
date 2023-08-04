using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace LogConverterTests
{
    [TestClass]
    public class LogConverterTests
    {
        [TestMethod]
        public void TestConvert()
        {
            // Arrange
            string input = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";
            string expectedOutput = "GET 200 /robots.txt 100.2 312 HIT";

            string inputPath = Path.GetTempFileName();
            File.WriteAllText(inputPath, input);

            string outputPath = Path.GetTempFileName();

            try
            {
                // Act
                LogConverter.Convert(inputPath, outputPath);

                // Assert
                string[] outputLines = File.ReadAllLines(outputPath);
                Assert.AreEqual(1, outputLines.Length);
                Assert.AreEqual(expectedOutput, outputLines[0]);
            }
            finally
            {
                File.Delete(inputPath);
                File.Delete(outputPath);
            }
        }

        [TestMethod]
        public void TestGetCacheStatus()
        {
            // Arrange
            string hitStatus = "HIT";
            string missStatus = "MISS";
            string invalidateStatus = "INVALIDATE";
            string unknownStatus = "UNKNOWN";

            // Act & Assert
            Assert.AreEqual("HIT", LogConverter.GetCacheStatus(hitStatus));
            Assert.AreEqual("MISS", LogConverter.GetCacheStatus(missStatus));
            Assert.AreEqual("REFRESH_HIT", LogConverter.GetCacheStatus(invalidateStatus));
            Assert.AreEqual("UNKNOWN", LogConverter.GetCacheStatus(unknownStatus));
        }
    }
}
