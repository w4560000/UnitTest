using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using WeihanLi.Extensions;

namespace MSTests.Helper
{
    internal static class Compare
    {
        public static void AreExpectedValueEqual(object? expected, object? actual)
        {
            if (object.Equals(expected, actual))
            {
                return;
            }
            else if (expected != null && actual != null)
            {
                // 比對
            }
            else if (expected == null || actual == null)
            {
                throw new Exception($"值為 null Expected: {JsonSerializer.Serialize(expected)}, Actual: {JsonSerializer.Serialize(actual)}");
            }

            var expectedType = expected.GetType();
            var actualType = actual.GetType();
            var isIEnumerable = expectedType.GetImplementedInterfaces()
                .Where(i => i.Name.StartsWith("IEnumerable"))
                .Count() > 0;
            isIEnumerable = (isIEnumerable || expectedType.IsArray);
            // 方便部分 IEnumerable 比對，陣列的不判斷型態
            // 目前不保證 IEnumerable 比對
            if (!isIEnumerable && expectedType.Name != actualType.Name)
            {
                throw new Exception($"錯誤型態 Expected: {expectedType.Name}, Actual: {actualType.Name}");
            }

            // 有實作 =
            var equalOp = expectedType.GetMethods()
                .Where(m => m.Name == "op_Equality")
                .FirstOrDefault();
            if (equalOp != null)
            {
                var compareResult = equalOp.Invoke(expected, new object[] { expected, actual }) ?? false;
                if ((bool)compareResult)
                {
                    return;
                }
                throw new Exception($"Expected: {JsonSerializer.Serialize(expected)}, Actual: {JsonSerializer.Serialize(actual)}");
            }

            if (expectedType.IsArray)
            {
                var expectedArray = expected as IEnumerable<object>;
                var actualArray = actual as IEnumerable<object>;
                if (expectedArray == null || actualArray == null)
                {
                    throw new Exception($"Expected: {JsonSerializer.Serialize(expected)}, Actual: {JsonSerializer.Serialize(actual)}");
                }
                if (expectedArray.Count() != actualArray.Count())
                {
                    throw new Exception($"Expected: {JsonSerializer.Serialize(expected)}, Actual: {JsonSerializer.Serialize(actual)}");
                }
                expectedArray.ForEach((expected, i) =>
                {
                    try
                    {
                        AreExpectedValueEqual(expected, actualArray.ElementAt(i));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Index: {i} {ex.Message}");
                    }
                });
                return;
            }

            var expectedProperties = expectedType.GetProperties();
            var actualPropertieDic = actualType.GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(actual));
            expectedProperties.ForEach(m =>
            {
                if (!m.CanRead)
                {
                    return;
                }

                var expectedValue = m.GetValue(expected);
                var actualValue = actualPropertieDic[m.Name];
                try
                {
                    AreExpectedValueEqual(expectedValue, actualValue);
                }
                catch (Exception ex)
                {
                    throw new Exception($"{m.Name} {ex.Message}");
                }
            });
        }

        public static bool IsCheckOk(Action checkFnc)
        {
            try
            {
                checkFnc();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}