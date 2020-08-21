## Selected framework and programming language
Dotnet core framework (version 3.1) and C# language (version 8.0) are used to solve the problem. xUnit frmaework is used for the test project.
## Project Structure 
You can find the solution in https://github.com/rikasumaiya/iptiQ-EMEA-P-C-coding-assessment </br>
The solution consists of two projects - LoadBalancerProblem and LoadBalancerTestProject.    
- LoadBalancerProblem project contains all the models, interfaces, and their implementations.
- LoadBalancerTestProject is the xUnit test project to test all the implementations of LoadBalancerProblem.
## How to run using travis CI
Go to the link https://travis-ci.org/github/rikasumaiya/iptiQ-EMEA-P-C-coding-assessment. You can find the latest build report there.
At the bottom of the report, you will see how many test cases passed out of total number of test cases. 
> A total of 1 test files matched the specified pattern. </br>
>Test Run Successful.</br>
>Total tests: 17 </br>
> Passed: 17</br>
>Total time: 4.1878 Seconds</br>
>The command "dotnet test LoadBalancerTestProject/LoadBalancerTestProject.csproj" exited with 0.</br>
## How to run locally
Travis CI has the restart build option only for the owner account. Hence, to run the solution and test cases, you need to clone the repository.
- You need visual studio 2019 installed on your system to run the solution.
- After installing the IDE click on  the LoadBalancerProblem.sln to open the solution. 
- Open Test explorer and run all the test cases.
## Number of test cases
There are in total 17 test cases which cover all the edge cases of the solution. 
