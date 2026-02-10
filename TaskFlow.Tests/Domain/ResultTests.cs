using FluentAssertions;
using TaskFlow.Domain.Common;

namespace TaskFlow.Tests.Domain
{
    public class ResultTests
    {
        [Fact]
        public void Success_ShouldCreateSuccessResult()
        {
            // Act
            var result = Result.Success();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Error.Should().BeNull();
        }

        [Fact]
        public void Failure_ShouldCreateFailureResult()
        {
            // Arrange
            var errorMessage = "Something went wrong";

            // Act
            var result = Result.Failure(errorMessage);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(errorMessage);
        }

        [Fact]
        public void Success_WithValue_ShouldCreateSuccessResultWithValue()
        {
            // Arrange
            var value = "test value";

            // Act
            var result = Result.Success(value);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Error.Should().BeNull();
            result.Value.Should().Be(value);
        }

        [Fact]
        public void Failure_WithType_ShouldCreateFailureResultWithType()
        {
            // Arrange
            var errorMessage = "Not found";

            // Act
            var result = Result.Failure<string>(errorMessage);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(errorMessage);
            result.Value.Should().BeNull();
        }

        [Fact]
        public void Failure_WithNullError_ShouldThrowException()
        {
            // Act
            Action act = () => Result.Failure(null!);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("A failed result must have an error.");
        }
    }
}
