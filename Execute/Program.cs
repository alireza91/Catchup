// See https://aka.ms/new-console-template for more information

Catchup.CatchupBusiness catchupBusiness = new Catchup.CatchupBusiness();

var puzzle = catchupBusiness.GetACaptcha();

Console.WriteLine(puzzle);
var answer = "";
bool result = false;

do
{
	answer = Console.ReadLine();
	result = catchupBusiness.CheckCaptcha(puzzle, answer);
	Console.WriteLine(result);
}
while (!result);
