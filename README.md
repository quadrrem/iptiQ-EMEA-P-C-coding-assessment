## Selected framework and programming language
Dotnet core framework (version 3.1) and C# language (version 8.0) are used to solve the problem. xUnit frmaework is used for the test project.
## Project Structure 
You can find the solution in https://github.com/rikasumaiya/iptiQ-EMEA-P-C-coding-assessment </br>
The solution consists of two projects - LoadBalancerProblem and LoadBalancerTestProject.    
- LoadBalancerProblem project contains all the models, interfaces, and their implementations.
- LoadBalancerTestProject is the xUnit test project to test all the implementations of LoadBalancerProblem.
## How to run using travis CI
### Step-1: 
Go to the link https://travis-ci.org/github/rikasumaiya/iptiQ-EMEA-P-C-coding-assessment. 
### Step-2: 
Click on the "Restart Build". It will start the build of the application and execute all the test cases. You can also find the latest ci pipeline result there.
### Step-3: 
Click on this link (https://travis-ci.org/github/rikasumaiya/iptiQ-EMEA-P-C-coding-assessment) to see the build report. 
There you will see how many test cases are passed out of the total number of test cases in the log file like below.
> A total of 1 test files matched the specified pattern. </br>
>Test Run Successful.</br>
>Total tests: 17 </br>
> Passed: 17</br>
>Total time: 4.1878 Seconds</br>
>The command "dotnet test LoadBalancerTestProject/LoadBalancerTestProject.csproj" exited with 0.</br>
## Number of test cases
There are in total 17 test cases which cover all the edge cases of the solution. 
