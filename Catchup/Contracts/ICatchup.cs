using Catchup.Models;
using System.Drawing;

namespace Catchup.Contracts;

public interface ICatchup
{
    //string GetACaptcha();
    ////BitmapResultModel GetAnImageCaptchaInBitmapFormat();
    //ByteArrayResultModel GetAnImageCaptchaInByteArray();
    //bool CheckCaptcha(string words, string number);

    /// <summary>
    /// Generates a captcha image along with its corresponding text.
    /// </summary>
    /// <param name="options">
    /// Optional configuration for captcha generation (length, colors, noise, etc.). 
    /// If null, default options will be used.
    /// </param>
    /// <returns>
    /// A <see cref="ByteArrayResultModel"/> containing the generated captcha image (as a byte array) 
    /// and the captcha text.
    /// </returns>
    public ByteArrayResultModel GetAnImageCaptcha(CaptchaOptions? options = null);

    /// <summary>
    /// Validates whether the provided solution matches the given captcha riddle.
    /// </summary>
    /// <param name="riddle">The original captcha text that was generated.</param>
    /// <param name="solution">The user-provided input to validate against the captcha.</param>
    /// <returns>
    /// True if the solution matches the riddle (case-insensitive, trimmed); otherwise, false.
    /// </returns>
    bool CheckCaptcha(string riddle, string solution);
}