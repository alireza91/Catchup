## Catchup
Catchup is a simple nuget package made in with .Net Core for Persians

# Description
Catchup produces a random 4 digits number in words as a string to be read by human beings.
Users must enter the exact match of the number generated to become validated as a non-robotic user.

## Setup
I recommend to inject the interface "ICaptcha" as a dependency so you can access the methods use them inside your "business layer".

## How it works
# Get a captcha
* To get a generated random captcha, use GetACaptcha() method.
This method takes no argument and return a string which contains 4 digits number in words as a captcha riddle.

# Solve the captcha
* To validate user as a non-robotic one, use CheckCaptcha(string riddle, string solution).
This method accepts two required arguments as the riddle and solution of it. the first one is the string you got from GetACaptcha() method and the second one (solution) is the number user sends as the answer. the method returns a boolean which determines if the answer is correct or not.

## Important
It is obvious that to prevent from attacks, you better store the generated captcha with the user who is requesting for it, so the attacker can't send you the similar riddle and his solution. This package is not doing it as it maybe differences in different project flows.

## From author
Of course that this package is not perfect and can be extended in many ways. Please FEEL FREE TO CONTRIBUTE, DEVELOP AND FIX BUGS by a pull request. I'd be glad to hear your suggestions or anything else from you through alireza_mortezaei@hotmail.com

