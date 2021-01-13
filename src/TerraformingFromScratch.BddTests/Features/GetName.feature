Feature: GetName
	In order to greet visitors
	As a user
	I want to receive a custom message

@getName
Scenario Outline: Receive a custom message
	Given I have assigned '<paramValue>' to the query string parameter '<paramName>'
	When I request the GetName API
	Then the response should be '<expectedResponse>'

	Examples: 
		| paramValue | paramName | expectedResponse                                                 |
		| Luke       | name      | Hello, Luke. This HTTP triggered function executed successfully. |
		| Leia       | name      | Hello, Leia. This HTTP triggered function executed successfully. |

@getName
Scenario Outline: Receive an error message
	Given I send an incorrect '<paramName>' in the query string
	When I request the GetName API
	Then the response should be 'This HTTP triggered function executed successfully. Pass a name in the query string for a personalized response.'

	Examples: 
		| paramName |
		| another   |
		|           |
