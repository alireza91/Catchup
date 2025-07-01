# Catchup
Catchup is a simple nuget package made with C#.

## Description
Catchup produces a random 6 characters Alphanumeric string to be read by human beings.
Users must enter the exact match of the text generated to become validated as a non-robotic user.

# Setup
I recommend to inject the interface "ICatchup" as a dependency so you can access the methods and use them inside your "Business layer".

# How it works
## Get a captcha in image as byte array
* To get a generated random captcha image, use GetAnImageCaptcha() method.
This method returns an object which contains a captcha string and a byte array that you can use directly in image tag.

* You can also customize the output by passing an optional `CaptchaOptions` object to the method:
```csharp
var customOptions = new CaptchaOptions
{
    Width = 250,
    Height = 100,
    FontSize = 50,
    CaptchaLength = 5,
    LineNoiseCount = 20,
    DotNoiseCount = 300
};

var captcha = catchup.GetAnImageCaptcha(customOptions);

```

## Examples in UI
* Example in html pages:
-> \<img style="width:500px;" src='data:image/png;base64, ' + imageInByteArray" />

* Example in razor pages:
-> <img style="width:500px;" src="@String.Format("data:image/png;base64,{0}", Convert.ToBase64String(imageInByteArray))" />"

## Solve the captcha
* To validate user as a non-robotic one, use CheckCaptcha(string riddle, string solution).
This method accepts two required arguments as the riddle and solution of it. the first one is the string you got from GetAnImageCaptcha() method and the second one (solution) is the number user sends as the answer. the method returns a boolean which determines if the answer is correct or not.

# Important
It is obvious that to prevent from attacks, you better store the generated captcha with the user who is requesting for it, so the attacker can't send you the similar riddle and his solution. This package is not doing it as it maybe differences in different project flows.

# From author
Of course that this package is not perfect and can be extended in many ways. Please FEEL FREE TO CONTRIBUTE, DEVELOP AND FIX BUGS by a pull request. I'd be glad to hear your suggestions or anything else from you through alireza_mortezaei@hotmail.com
