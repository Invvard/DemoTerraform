Feature: DoWePush
	Simple function to know if a date is good for a deployment in production

@functionTest
@getRequest
Scenario: Check if a monday is a good day with a GET
	Given the query string parameter 'date'
	And the date '2021-01-04'
	When I get the function response
	Then the response message should be 'This is the way'
	
@functionTest
@getRequest
Scenario: Check if a friday is NOT a good day with a GET
	Given the query string parameter 'date'
	And the date '2021-01-01'
	When I get the function response
	Then the response message should be 'This is madness !'
	
@functionTest
@postRequest
Scenario: Check if a monday is a good day with a POST
	Given the body content parameter 'date'
	And the date '2021-01-04'
	When I post the function response
	Then the request result should be 'OK' 
	And the response message should be 'This is the way'
	
@functionTest
@getRequest
Scenario Outline: Check which days are GO to production days
	Given the query string parameter 'date'
	And the date '<dateParameter>'
	When I get the function response
	Then the response message should be '<expectedResponse>'

	Examples: 
		| dateParameter | expectedResponse  |
		| 2021-01-01    | This is madness ! |
		| 2021-01-02    | This is the way   |
		| 2021-01-03    | This is the way   |
		| 2021-01-04    | This is the way   |