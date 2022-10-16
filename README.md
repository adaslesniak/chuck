# Azure Workshop
My first serverless project.
Simple Azure function that browse some page go get some data.

### Requirements:
  * Collect data and store it in SQLite.
  * Implement time triggered Azure function which will be triggered  
       every configurable X minutes and pull X jokes from external service: 
	   https://rapidapi.com/matchilling/api/chucknorris/ 
  * Function should filter out jokes that have more than 200 characters.   
        Database should not contain any duplicated quotes. 
  * Application should allow to easily change jokes provider to other API.
  * Configure logger, error handler and cover the important parts with unit tests
  * Remember about SOLID principles
  
### Goal
The goal for the exercise it to present coding style,
compliance with SOLID principles as well as
knowledge of commonly used libraries in .NET framework.
 
 ### Afterthoughts:
   **Bad business directives does matter always, even in simplest project.**
> In this case generic joke gatherer is against exact requirments 
> to pull exact amount of jokes with api that does not support paging. 
> Here is conflict between requirments and good architecture. 
> Without caching what was collected any collection makes no sense, 
> as it will be just random calls for random stuff, 
> therefore any other optimization seemed to have no sense (async calls, paralel calls). 
> Just by making little bigger calls (call for categhory, not single joke) would allow 
> some caching and avoid repeated calls, and make IChuckTrace more generic.

    