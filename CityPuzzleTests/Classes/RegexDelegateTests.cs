using CityPuzzle.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CityPuzzle.Classes.Tests
{
    public class RegexDelegateTests
    {
        [Theory]
        [InlineData("asd")]
        [InlineData("asdfghjk!")]
        public void RegexDelegate_ValidUsernameFail(string input)
        {
            bool isValid = RegexDelegate.ValidUsername(input);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData("Cheburashka")]
        [InlineData("krokodilas")]
        public void RegexDelegate_ValidUsernamePass(string input)
        {
            bool isValid = RegexDelegate.ValidUsername(input);

            Assert.True(isValid);
        }

        [Theory]
        [InlineData("Abc1")]
        [InlineData("Abcdefgh")]
        [InlineData("abcdefg1")]
        public void RegexDelegate_ValidPasswordFail(string input)
        {
            bool isValid = RegexDelegate.ValidPassword(input);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData("Abcdefg1")]
        public void RegexDelegate_ValidPasswordPass(string input)
        {
            bool isValid = RegexDelegate.ValidPassword(input);

            Assert.True(isValid);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("Abcdefgh@")]
        [InlineData("Abcdefgh")]
        public void RegexDelegate_ValidEmailFail(string input)
        {
            bool isValid = RegexDelegate.ValidEmail(input);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData("abcdefgh@gmail.com")]
        [InlineData("abcdefgh@yahoo.com")]
        
        public void RegexDelegate_ValidEmailPass(string input)
        {
            bool isValid = RegexDelegate.ValidEmail(input);

            Assert.True(isValid);
        }
    }
}