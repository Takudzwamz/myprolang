// initialize the variable n
n = 9;
// function to calculate the factorial of a number
function factorial(n) {
  if (n == 0) {
    return 1;
  } else {
    return n * factorial(n - 1);
  }
}

Write("Factorial of n is: " + factorial(n)); // print the result to the console
