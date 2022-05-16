# Catchup
Catchup is a simple nuget package made with .Net Core for Persians

## Description
Catchup produces a random 4 digits number in words as a string to be read by human beings.
Users must enter the exact match of the number generated to become validated as a non-robotic user.

# Setup
I recommend to inject the interface "ICatchup" as a dependency so you can access the methods and use them inside your "Business layer".

# How it works
## Get a captcha as string
* To get a generated random captcha, use GetACaptcha() method.
This method takes no argument and return a string which contains 4 digits number in words as a captcha riddle.

## Get a captcha in image with Bitmap format
* To get a generated random captcha image, use GetAnImageCaptchaInBitmapFormat() method.
This method returns an object from Bitmap and the captcha string class in C#.

## Get a captcha in image as byte array
* To get a generated random captcha image, use GetAnImageCaptchaInByteArray() method.
This method returns an object which a byte array that you can use directly in <img /> tag and a captcha string.

* Example in html page:
-> \<img style="width:500px;" src="data:image/png;base64, imageInByteArray)" />

* Example in razor pages:
-> <img style="width:500px;" src="@String.Format("data:image/png;base64,{0}", Convert.ToBase64String(imageInByteArray))" />"

## Solve the captcha
* To validate user as a non-robotic one, use CheckCaptcha(string riddle, string solution).
This method accepts two required arguments as the riddle and solution of it. the first one is the string you got from GetACaptcha() method and the second one (solution) is the number user sends as the answer. the method returns a boolean which determines if the answer is correct or not.

# Important
It is obvious that to prevent from attacks, you better store the generated captcha with the user who is requesting for it, so the attacker can't send you the similar riddle and his solution. This package is not doing it as it maybe differences in different project flows.

# From author
Of course that this package is not perfect and can be extended in many ways. Please FEEL FREE TO CONTRIBUTE, DEVELOP AND FIX BUGS by a pull request. I'd be glad to hear your suggestions or anything else from you through alireza_mortezaei@hotmail.com